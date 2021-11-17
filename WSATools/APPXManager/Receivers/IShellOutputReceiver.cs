﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPXManager.Receivers
{
    /// <summary>
    ///
    /// </summary>
    public interface IShellOutputReceiver
    {
        /// <summary>
        /// Gets a value indicating whether the receiver parses error messages.
        /// </summary>
        /// <value>
        ///     <see langword="true"/> if this receiver parsers error messages; otherwise <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// The default value is <see langword="false"/>. If set to <see langword="false"/>, the <see cref="AdvancedAdbClient"/>
        /// will detect common error messages and throw an exception.
        /// </remarks>
        bool ParsesErrors { get; }

        /// <summary>
        /// Adds the output.
        /// </summary>
        void AddOutput(string line);

        /// <summary>
        /// Flushes the output.
        /// </summary>
        /// <remarks>This should always be called at the end of the "process" in order to indicate that the data is ready to be processed further if needed.</remarks>
        void Flush();
    }
}
