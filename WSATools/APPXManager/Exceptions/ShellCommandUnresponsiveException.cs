﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace APPXManager.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a shell command becomes unresponsive.
    /// </summary>
    public class ShellCommandUnresponsiveException : AdbException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellCommandUnresponsiveException"/> class.
        /// </summary>
        public ShellCommandUnresponsiveException()
            : base("The shell command has become unresponsive")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellCommandUnresponsiveException"/> class with a
        /// specified error message.
        /// </summary>
        /// <param name="message">
        /// The message that describes the error.
        /// </param>
        public ShellCommandUnresponsiveException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellCommandUnresponsiveException"/> class with a
        /// reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="inner">
        /// The exception that is the cause of the current exception, or
        /// <see langword="null"/> if no inner exception is specified.
        /// </param>
        public ShellCommandUnresponsiveException(Exception inner)
            : base("The shell command has become unresponsive", inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellCommandUnresponsiveException"/> class with a
        /// specified error message and a reference to the inner exception that is the cause
        /// of this exception.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="inner">
        /// The exception that is the cause of the current exception, or
        /// <see langword="null"/> if no inner exception is specified.
        /// </param>
        public ShellCommandUnresponsiveException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellCommandUnresponsiveException"/> class with serialized
        /// data.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo"/> that holds the serialized object data about the
        /// exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"/> that contains contextual information about the
        /// source or destination.
        /// </param>
        internal ShellCommandUnresponsiveException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}