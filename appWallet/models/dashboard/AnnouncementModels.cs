using System;
using System.Collections.Generic;

namespace IAM_Library.appWallet.models.dashboard
{
    /// <summary>
    /// Announcement row returned by GET announcement endpoints (see Announcements API documentation).
    /// </summary>
    public class WalletAnnouncementItem
    {
        public int announcementID { get; set; }
        public string title { get; set; }
        public int announcementType { get; set; }
        public string announcementContent { get; set; }
        public string mediaFiles { get; set; }
        public int urgencyType { get; set; }
        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }
        public bool status { get; set; }
        public DateTime? updatedAt { get; set; }
        public string updatedBy { get; set; }
    }

    /// <summary>
    /// Response body for Create / Update announcement operations (statusCode 1 = success, 0 = error).
    /// </summary>
    public class AnnouncementOperationResponse
    {
        public int statusCode { get; set; }
        public string message { get; set; }
    }

    /// <summary>
    /// JSON body for PUT /v1/Announcements/UpdateAnnouncement.
    /// </summary>
    public class UpdateAnnouncementRequest
    {
        public int announcementID { get; set; }
        public string title { get; set; }
        public int announcementType { get; set; }
        public string announcementContent { get; set; }
        public int urgencyType { get; set; }
        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }
        public bool status { get; set; }
        public string updatedBy { get; set; }
    }

    /// <summary>
    /// Fields for POST /v1/Announcements/CreateAnnouncement (multipart/form-data). Optional file paths are uploaded as MediaFiles.
    /// </summary>
    public class CreateAnnouncementRequest
    {
        public string Title { get; set; }
        public int AnnouncementType { get; set; }
        public string AnnouncementContent { get; set; }
        public int UrgencyType { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public bool Status { get; set; } = true;
        public string UpdatedBy { get; set; }
        public IReadOnlyList<string> MediaFilePaths { get; set; }
    }
}
