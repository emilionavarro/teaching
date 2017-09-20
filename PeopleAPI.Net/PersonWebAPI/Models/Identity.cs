using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonWebAPI.Models
{
    public abstract class Identity
    {
        /// <summary>
        /// Variables
        /// </summary>
        private static int Counter = 0;
        public int Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Constructors
        /// </summary>
        public Identity()
        {
            Id = Counter++;
        }

        public Identity(int inId, string inName)
        {
            Id = inId;
            Name = inName;
        }
    }
}