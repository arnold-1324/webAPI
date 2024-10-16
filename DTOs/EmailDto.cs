using System;

namespace twitterclone.DTOs;

public class EmailDto
{
    public required string ToEmail { get; set; }
    public required string Subject { get; set; }
    public required string Body { get; set; }
}
