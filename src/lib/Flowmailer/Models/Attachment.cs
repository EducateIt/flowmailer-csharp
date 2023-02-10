using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowmailer.Models
{
    public class Attachment
    {
        /// <summary>
        /// Content as Bas64
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Content-ID header (required for disposition related)
        /// </summary>
        public string ContentId { get; set; }

        /// <summary>
        /// MIME type of the content
        /// Examples: application/pdf, image/jpeg
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Content-Disposition header for the attachment.
        /// Supported values include: attachment, inline and related
        /// The type of disposition, <see cref="DispositionTypes"/>
        /// </summary>
        public string ContentDisposition { get; set; }

        /// <summary>
        /// The filename
        /// </summary>
        public string Filename { get; set; }
    }
}
