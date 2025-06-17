using FreedomLogic.Identity;
using FreedomLogic.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreedomLogic.Entities
{
    [Table("modelviewer_models")]
    public class ModelViewerModelData
    {
        [Key]
        [Column("id")]
        public uint Id { get; set; }
        [Column("type")]
        [MaxLength(3)]
        public string Type { get; set; }
        [Column("fileName")]
        public string FileName { get; set; }

        public virtual List<ModelViewerModelToTag> Tags { get; set; }
        public virtual List<ModelViewerModelToCollection> Collections { get; set; }
    }

    [Table("modelviewer_tag")]
    public class ModelViewerTag : EntityBase
    {
        [Key]
        [Column("id")]
        public override int Id { get; set; }
        [Column("tag")]
        [MaxLength(25)]
        public string Tag { get; set; }

        public virtual List<ModelViewerModelToTag> Models { get; set; }
    }

    [Table("modelviewer_model_to_tag")]
    public class ModelViewerModelToTag
    {
        [Key]
        [Column("model_id")]
        public uint ModelId { get; set; }
        [Key]
        [Column("tag_id")]
        public int TagId { get; set; }
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ModelViewerTag Tag { get; set; }
        public virtual ModelViewerModelData Model { get; set; }
    }

    [Table("modelviewer_collection")]
    public class ModelViewerCollection : EntityBase
    {
        [Key]
        [Column("id")]
        public override int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("name")]
        [MaxLength(250)]
        public string Name { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<ModelViewerModelToCollection> Models { get; set; }
    }

    [Table("modelviewer_model_to_collection")]
    public class ModelViewerModelToCollection
    {
        [Key]
        [Column("model_id")]
        public uint ModelId { get; set; }
        [Column("collection_id")]
        public int CollectionId { get; set; }
        public virtual ModelViewerCollection Collection { get; set; }
        public virtual ModelViewerModelData Model { get; set; }
    }
}
