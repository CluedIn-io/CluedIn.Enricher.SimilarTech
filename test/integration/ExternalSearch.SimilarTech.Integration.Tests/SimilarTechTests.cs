using CluedIn.Core.Data;
using CluedIn.Core.Data.Parts;
using CluedIn.Core.Messages.Processing;
using CluedIn.ExternalSearch.Providers.SimilarTech;
using CluedIn.Testing.Base.ExternalSearch;
using Moq;
using Xunit;

namespace ExternalSearch.SimilarTech.Integration.Tests
{
    public class SimilarTechTests : BaseExternalSearchTest<SimilarTechExternalSearchProvider>
    {
        [Fact(Skip = "Execute only when token is in the configuration")]
        public void TestClueProduction()
        {
            var dummy = new DummyTokenProvider("f5217552d87b4b92a8a8f47f81f462b0");
            object[] parameters = { dummy };
            
            var properties = new EntityMetadataPart();
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.Website, "sitecore.com");

            IEntityMetadata entityMetadata = new EntityMetadataPart() {
                Name = "Sitecore",
                EntityType = EntityType.Organization,
                Properties = properties.Properties
            };

            this.Setup(parameters, entityMetadata);

            this.testContext.ProcessingHub.Verify(h => h.SendCommand(It.IsAny<ProcessClueCommand>()), Times.AtLeastOnce);
            Assert.True(this.clues.Count > 0);
        }
    }
}