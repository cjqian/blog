using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class Comment
    {
        public readonly int MIN_LENGTH = 2;
        public readonly int MAX_LENGTH = 1000;

        public string Author { get; set; }

        // For content validation, make sure that the content is within a reasonable length. 
        [Required]
        [StringLength(1000, ErrorMessage = "The comment have at least (2) characters and must not exceed (1000) characters.", MinimumLength = 2)]
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public int EntryID { get; set; }
        public int ID { get; set; }
    }
}
