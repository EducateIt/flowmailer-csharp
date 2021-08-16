namespace Flowmailer.Models
{
    /// <summary>
    /// Holder for header key value pairs
    /// </summary>
    public class Header
    {
        /// <summary>
        /// Name of the kvp
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Value of the kvp
        /// </summary>
        public string Value { get; set; }
    }
}