// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompanyVocabulary.cs" company="Clued In">
//   Copyright (c) 2018 Clued In. All rights reserved.
// </copyright>
// <summary>
//   Implements the company vocabulary class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using CluedIn.Core.Data;
using CluedIn.Core.Data.Vocabularies;

namespace CluedIn.ExternalSearch.Providers.SimilarTech.Vocabularies
{
    public class CompanyVocabulary : SimpleVocabulary
    {
        public CompanyVocabulary()
        {
            this.VocabularyName = "SimilarTech Organization";
            this.KeyPrefix = "SimilarTech.Organization";
            this.KeySeparator = ".";
            this.Grouping = EntityType.Organization;

            this.AddGroup("SimilarTech Company Social Media", group =>
            {
                this.LinkedInId    = group.Add(new VocabularyKey("LinkedIn Id"));
                this.FacebookLikes    = group.Add(new VocabularyKey("Facebook Likes"));
                this.TwitterFollowers = group.Add(new VocabularyKey("Twitter Followers"));
            });

            this.AddGroup("SimilarTech Company Details", group =>
            {
                this.Homepage    = group.Add(new VocabularyKey("Homepage"));
                this.Favicon     = group.Add(new VocabularyKey("Favicon"));
                this.Logo        = group.Add(new VocabularyKey("Logo"));
                this.Rankings    = group.Add(new VocabularyKey("Rankings"));
                this.Location    = group.Add(new VocabularyKey("Location"));
                this.Description = group.Add(new VocabularyKey("Description"));
            });

            this.Site          = this.Add(new VocabularyKey("Site"));
            this.Name          = this.Add(new VocabularyKey("Name"));
            this.FacebookUrl   = this.Add(new VocabularyKey("FacebookUrl"));
            this.TwitterUrl    = this.Add(new VocabularyKey("TwitterUrl"));
            this.LinkedInUrl   = this.Add(new VocabularyKey("LinkedInUrl"));
            this.Address       = this.Add(new VocabularyKey("Address"));
            this.RevenueRange  = this.Add(new VocabularyKey("RevenueRange"));
            this.EmployeeRange = this.Add(new VocabularyKey("EmployeeRange"));
            this.Zip           = this.Add(new VocabularyKey("Zip"));
            this.YearFounded   = this.Add(new VocabularyKey("YearFounded"));
            this.City          = this.Add(new VocabularyKey("City"));
            this.State         = this.Add(new VocabularyKey("State"));
            this.Fax           = this.Add(new VocabularyKey("Fax"));
            this.Phone         = this.Add(new VocabularyKey("Phone"));
            this.Country       = this.Add(new VocabularyKey("Country"));

            this.AddMapping(this.Site, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.Website);
            this.AddMapping(this.Name, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.OrganizationName);
            this.AddMapping(this.FacebookUrl, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.Social.Facebook);
            this.AddMapping(this.TwitterUrl, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.Social.Twitter);
            this.AddMapping(this.LinkedInUrl, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.Social.LinkedIn);
            this.AddMapping(this.Address, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.Address);
            this.AddMapping(this.RevenueRange, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AnnualRevenue);
            this.AddMapping(this.EmployeeRange, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.EmployeeCount);
            this.AddMapping(this.YearFounded, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.FoundingDate);
            this.AddMapping(this.Zip, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressZipCode);
            this.AddMapping(this.City, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressCity);
            this.AddMapping(this.State, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressState);
            this.AddMapping(this.Fax, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.Fax);
            this.AddMapping(this.Phone, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.PhoneNumber);

        }

        public VocabularyKey Site { get; set; }
        public VocabularyKey Name { get; set; }
        public VocabularyKey Description { get; set; }
        public VocabularyKey YearFounded { get; set; }
        public VocabularyKey Homepage { get; set; }
        public VocabularyKey Favicon { get; set; }
        public VocabularyKey Logo { get; set; }
        public VocabularyKey Rankings { get; set; }
        public VocabularyKey Address { get; set; }
        public VocabularyKey Location { get; set; }
        public VocabularyKey Country { get; set; }
        public VocabularyKey State { get; set; }
        public VocabularyKey City { get; set; }
        public VocabularyKey Zip { get; set; }
        public VocabularyKey Phone { get; set; }
        public VocabularyKey Fax { get; set; }
        public VocabularyKey EmployeeRange { get; set; }
        public VocabularyKey RevenueRange { get; set; }
        public VocabularyKey LinkedInUrl { get; set; }
        public VocabularyKey FacebookUrl { get; set; }
        public VocabularyKey TwitterUrl { get; set; }
        public VocabularyKey FacebookLikes { get; set; }
        public VocabularyKey TwitterFollowers { get; set; }
        public VocabularyKey LinkedInId { get; set; }
      
    }
}
