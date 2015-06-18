using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Validate
{
    /// <summary>
    /// Describes the result of a validation of a potential change through a business service.
    /// </summary>
    public class ValidateResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        public ValidateResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        /// <param name="memeberName">Name of the memeber.</param>
        /// <param name="message">The message.</param>
        public ValidateResult(string memeberName, string message)
        {
            this.MemberName = memeberName;
            this.Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ValidateResult(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets or sets the name of the member.
        /// </summary>
        /// <value>
        /// The name of the member.  May be null for general validation issues.
        /// </value>
        public string MemberName { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }
    }
}
