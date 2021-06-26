using System;
using System.ComponentModel.DataAnnotations;

namespace RandomMediaPlayer.Storage.Models
{
    /// <summary>
    /// Stores history of watcher media
    /// </summary>
    public class UriHistory
    {
        /// <summary>
        /// Base path of a media source
        /// </summary>
        [Required]
        public string BasePath { get; set; }
        /// <summary>
        /// Specific name of an entity inside <see cref="BasePath"/>
        /// </summary>
        public string EntityName { get; set; }
        /// <summary>
        /// Time at which the history entry was initially added
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime AddedAt { get; set; }
    }
}