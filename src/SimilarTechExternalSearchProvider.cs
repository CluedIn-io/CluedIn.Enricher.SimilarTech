// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimilarTechExternalSearchProvider.cs" company="Clued In">
//   Copyright (c) 2019 Clued In. All rights reserved.
// </copyright>
// <summary>
//   Implements the similar technology external search provider class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;

using CluedIn.Core;
using CluedIn.Core.Data;
using CluedIn.Core.Data.Parts;
using CluedIn.Core.ExternalSearch;
using CluedIn.Crawling.Helpers;
using CluedIn.ExternalSearch.Filters;
using CluedIn.ExternalSearch.Providers.SimilarTech.Models;
using CluedIn.ExternalSearch.Providers.SimilarTech.Vocabularies;

using RestSharp;

namespace CluedIn.ExternalSearch.Providers.SimilarTech
{
    public class SimilarTechExternalSearchProvider : ExternalSearchProviderBase
    {
        /**********************************************************************************************************
         * CONSTRUCTORS
         **********************************************************************************************************/

        public SimilarTechExternalSearchProvider()
            : base(new Guid("d40eca75-cdea-44f2-9132-730a5b2498ee"), EntityType.Organization)
        {
            this.TokenProvider = new NameBasedTokenProvider("SimilarTech");
        }

        public SimilarTechExternalSearchProvider(string token)
            : base(new Guid("d40eca75-cdea-44f2-9132-730a5b2498ee"), EntityType.Organization)
        {
            this.TokenProvider = new NameBasedTokenProvider(token);
        }
 
        public SimilarTechExternalSearchProvider([NotNull] IExternalSearchTokenProvider tokenProvider)
            : base(new Guid("d40eca75-cdea-44f2-9132-730a5b2498ee"), EntityType.Organization)
        {
            this.TokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
        }

        /**********************************************************************************************************
         * METHODS
         **********************************************************************************************************/

        public override IEnumerable<IExternalSearchQuery> BuildQueries(ExecutionContext context, IExternalSearchRequest request)
        {
            if (!this.Accepts(request.EntityMetaData.EntityType))
                yield break;

            var existingResults = request.GetQueryResults<SimilarTechResponse>(this).ToList();

            Func<string, bool> nameFilter = value => existingResults.Any(r => string.Equals(r.Data.Site, value, StringComparison.InvariantCultureIgnoreCase));

            var entityType = request.EntityMetaData.EntityType;
            var domain = request.QueryParameters.GetValue(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.Website, new HashSet<string>());

            if (domain != null)
            {
                var values = domain.Select(NameNormalization.Normalize).ToHashSet();

                foreach (var value in values.Where(v => !nameFilter(v)))
                    yield return new ExternalSearchQuery(this, entityType, ExternalSearchQueryParameter.Domain, value);
            }
        }

        public override IEnumerable<IExternalSearchQueryResult> ExecuteSearch(ExecutionContext context, IExternalSearchQuery query)
        {
            var domain = query.QueryParameters[ExternalSearchQueryParameter.Domain].FirstOrDefault();

            var apiToken = this.TokenProvider.ApiToken;

            if (string.IsNullOrEmpty(domain))
                yield break;

            var client = new RestClient("https://api.similartech.com/v1");

            var request = new RestRequest(string.Format("site/{0}/company", domain), Method.GET);

            if (string.IsNullOrEmpty(apiToken))
                throw new InvalidOperationException("SimilarTech ApiToken have not been configured");

            request.AddParameter("userkey", apiToken);

            var company = client.Execute<SimilarTechResponse>(request);
            var response = company;
 
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Data != null)
                {
                    request.Resource = string.Format("site/{0}/technologies", domain);
                    var technologiesResponse = client.ExecuteTaskAsync<TechnologiesResponse>(request).Result;

                    if (response.StatusCode == HttpStatusCode.OK)
                        response.Data.TechnologiesResponse = technologiesResponse.Data;

                    yield return new ExternalSearchQueryResult<SimilarTechResponse>(query, response.Data);
                }
            }
            else if (response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.NotFound)
                yield break;
            else if (response.ErrorException != null)
                throw new AggregateException(response.ErrorException.Message, response.ErrorException);
            else
                throw new ApplicationException("Could not execute external search query - StatusCode:" + response.StatusCode + "; Content: " + response.Content);
        }

        public override IEnumerable<Clue> BuildClues(ExecutionContext context, IExternalSearchQuery query, IExternalSearchQueryResult result, IExternalSearchRequest request)
        {
            var resultItem = result.As<SimilarTechResponse>();

            var code = this.GetOriginEntityCode(resultItem);

            var companyClue = new Clue(code, context.Organization);

            this.DownloadPreviewImage(context, new Uri(resultItem.Data.Logo), companyClue);
            this.PopulateMetadata(companyClue.Data.EntityData, resultItem);

            if (resultItem.Data.TechnologiesResponse.Technologies != null)
            {
                foreach (var tech in resultItem.Data.TechnologiesResponse.Technologies)
                {
                    if (tech == null)
                        continue;

                    var techCode = this.GetTechnologyOriginEntityCode(tech);

                    var techClue = new Clue(techCode, context.Organization);

                    this.PopulateTechnologyMetadata(techClue.Data.EntityData, tech, resultItem);

                    yield return techClue;
                }
            }

            yield return companyClue;
        }

        public override IEntityMetadata GetPrimaryEntityMetadata(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request)
        {
            var resultItem = result.As<SimilarTechResponse>();
            return this.CreateMetadata(resultItem);
        }

        private IEntityMetadata CreateMetadata(IExternalSearchQueryResult<SimilarTechResponse> resultItem)
        {
            var metadata = new EntityMetadataPart();

            this.PopulateMetadata(metadata, resultItem);

            return metadata;
        }

        private EntityCode GetOriginEntityCode(IExternalSearchQueryResult<SimilarTechResponse> resultItem)
        {
            return new EntityCode(EntityType.Organization, this.GetCodeOrigin(), resultItem.Data.Site);
        }

        private EntityCode GetTechnologyOriginEntityCode(Technology technology)
        {
            return new EntityCode(EntityType.Product, this.GetCodeOrigin(), technology.Id);
        }

        private CodeOrigin GetCodeOrigin()
        {
            return CodeOrigin.CluedIn.CreateSpecific("similartech");
        }

        private void PopulateMetadata(IEntityMetadata metadata, IExternalSearchQueryResult<SimilarTechResponse> resultItem)
        {
            var code = this.GetOriginEntityCode(resultItem);

            metadata.EntityType = EntityType.Organization;

            metadata.OriginEntityCode = code;

            metadata.Codes.Add(code);

            var vocab = new CompanyVocabulary();

            try { metadata.Uri = new Uri(resultItem.Data.Homepage); }
            catch { metadata.Properties[vocab.Homepage] = resultItem.Data.Homepage.PrintIfAvailable(); }

            if (resultItem.Data.Description != null)
                metadata.Description = resultItem.Data.Description;

            if (resultItem.Data.Name != null)
                metadata.Name = resultItem.Data.Name;

            metadata.Properties[vocab.Site]             = resultItem.Data.Site.PrintIfAvailable();
            metadata.Properties[vocab.Address]          = resultItem.Data.Address.PrintIfAvailable();
            metadata.Properties[vocab.Favicon]          = resultItem.Data.Favicon.PrintIfAvailable();
            metadata.Properties[vocab.Rankings]         = resultItem.Data.Rankings.PrintIfAvailable();
            metadata.Properties[vocab.Location]         = resultItem.Data.Location.PrintIfAvailable();
            metadata.Properties[vocab.Country]          = resultItem.Data.Country.PrintIfAvailable();
            metadata.Properties[vocab.State]            = resultItem.Data.State.PrintIfAvailable();
            metadata.Properties[vocab.City]             = resultItem.Data.City.PrintIfAvailable();
            metadata.Properties[vocab.Zip]              = resultItem.Data.Zip.PrintIfAvailable();
            metadata.Properties[vocab.Phone]            = resultItem.Data.Phone.PrintIfAvailable();
            metadata.Properties[vocab.Fax]              = resultItem.Data.Fax.PrintIfAvailable();
            metadata.Properties[vocab.EmployeeRange]    = resultItem.Data.EmployeeRange.PrintIfAvailable();
            metadata.Properties[vocab.RevenueRange]     = resultItem.Data.RevenueRange.PrintIfAvailable();
            metadata.Properties[vocab.LinkedInId]       = resultItem.Data.LinkedInId.PrintIfAvailable();
            metadata.Properties[vocab.LinkedInUrl]      = resultItem.Data.LinkedInUrl.PrintIfAvailable();
            metadata.Properties[vocab.FacebookUrl]      = resultItem.Data.FacebookUrl.PrintIfAvailable();
            metadata.Properties[vocab.FacebookLikes]    = resultItem.Data.FacebookLikes.PrintIfAvailable();
            metadata.Properties[vocab.TwitterFollowers] = resultItem.Data.TwitterFollowers.PrintIfAvailable();
            metadata.Properties[vocab.TwitterUrl]       = resultItem.Data.TwitterUrl.PrintIfAvailable();
            metadata.Properties[vocab.Logo]             = resultItem.Data.Logo.PrintIfAvailable();
            metadata.Properties[vocab.YearFounded]      = resultItem.Data.YearFounded.PrintIfAvailable();

        }

        private void PopulateTechnologyMetadata(IEntityMetadata metadata, Technology technology, IExternalSearchQueryResult<SimilarTechResponse> resultItem)
        {
            var code = this.GetTechnologyOriginEntityCode(technology);

            metadata.EntityType = EntityType.Product;

            metadata.OriginEntityCode = code;

            if (technology.Name != null)
                metadata.Name = technology.Name;

            var vocab = new TechnologyVocabulary();

            metadata.Codes.Add(code);
            metadata.Properties[vocab.Paying] = technology.Paying.PrintIfAvailable();
            metadata.Properties[vocab.Coverage] = technology.Coverage.ToString().PrintIfAvailable();
            metadata.Tags.AddRange(technology.Categories.Select(s => new Tag(s)));

            technology.Name = vocab.Technology;
            technology.Paying = vocab.Paying;

            var from = new EntityReference(code);
            var to = new EntityReference(this.GetOriginEntityCode(resultItem));

            var edge = new EntityEdge(from, to, EntityEdgeType.UsedBy);

            metadata.OutgoingEdges.Add(edge);

        }

        public override IPreviewImage GetPrimaryEntityPreviewImage(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request)
        {
            return this.DownloadPreviewImageBlob<SimilarTechResponse>(context, result, f => f.Data.Logo);
        }
    }
}