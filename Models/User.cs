using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
namespace twitterclone.Models;

public class User
{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        
        public   string? Id { get; set; }

        [BsonRequired]
        public required string Username { get; set; }

        [BsonRequired]
        public required string FullName { get; set; }

        [BsonRequired]
         [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public required string Password { get; set; }

        public DateTime LastLogin { get; set; } = DateTime.Now;

        public bool IsVerified { get; set; } = false;

        public  string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordExpires { get; set; }

        public required string VerificationToken { get; set; }
        public DateTime VerificationTokenExpiresAt { get; set; }

        [BsonRequired]
        [EmailAddress(ErrorMessage = "Invalid Email Address")] 
        //[UniqueEmail]
        public required string Email { get; set; }

        public List<string> Followers { get; set; } = new List<string>();
        public List<string> Following { get; set; } = new List<string>();
        public bool IsFrozen { get; set; } = false;

        public string ProfileImg { get; set; } = "";
        public string Bio { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
