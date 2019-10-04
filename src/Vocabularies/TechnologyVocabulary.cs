// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TechnologyVocabulary.cs" company="Clued In">
//   Copyright (c) 2018 Clued In. All rights reserved.
// </copyright>
// <summary>
//   Implements the technology vocabulary class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using CluedIn.Core.Data;
using CluedIn.Core.Data.Vocabularies;

namespace CluedIn.ExternalSearch.Providers.SimilarTech.Vocabularies
{
    public class TechnologyVocabulary : SimpleVocabulary
    {
        public TechnologyVocabulary()
        {
            this.VocabularyName = "SimilarTech Technology";
            this.KeyPrefix      = "SimilarTech.Technology";
            this.KeySeparator   = ".";
            this.Grouping       = EntityType.Product;

            this.AddGroup("SimilarTech Technologies", group =>
            {
                this.Coverage   = group.Add(new VocabularyKey("Coverage"));
                this.Id         = group.Add(new VocabularyKey("Id"));
                this.Technology = group.Add(new VocabularyKey("Name"));
                this.Categories = group.Add(new VocabularyKey("Categories"));
                this.Paying     = group.Add(new VocabularyKey("Paying"));
            });

        }

        public VocabularyKey Coverage { get; set; }
        public VocabularyKey Id { get; set; }
        public VocabularyKey Technology { get; set; }
        public VocabularyKey Categories { get; set; }
        public VocabularyKey Paying { get; set; }

    }
}
