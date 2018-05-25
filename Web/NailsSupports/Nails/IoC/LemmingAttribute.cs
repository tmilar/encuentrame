using System;

namespace NailsFramework.IoC
{
    [AttributeUsage(AttributeTargets.Class)]
    public class LemmingAttribute : Attribute
    {
        private bool singleton = true;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "InjectAttribute" /> class.
        /// </summary>
        public LemmingAttribute() : this(null)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "InjectAttribute" /> class.
        /// </summary>
        /// <param name = "name">Name of the lemming.</param>
        public LemmingAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        ///   Gets or sets a value indicating whether this <see cref = "LemmingAttribute" /> is inCache.
        /// </summary>
        /// <value><c>true</c> if inCache; otherwise, <c>false</c>.</value>
        public bool Singleton
        {
            get { return singleton; }
            set { singleton = value; }
        }

        /// <summary>
        ///   Gets or sets the name of the lemming.
        /// </summary>
        /// <value>The name of the lemming.</value>
        public string Name { get; set; }
    }
}