using EnvDTE80;
using System.Globalization;
using System.Reflection;
using System.Resources;
//using log4net;

namespace Randoop
{
    public abstract class CommandBase
    {
//        protected static readonly ILog _logger = LogManager.GetLogger((MethodBase.GetCurrentMethod().DeclaringType));
//        protected static ResourceManager rm = new ResourceManager("Explorer.Explorer", Assembly.GetExecutingAssembly());
        protected static ResourceManager rm = new ResourceManager("Randoop.CommandBar", Assembly.GetExecutingAssembly());
        protected static CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentUICulture;

        protected DTE2 application;
        private System.Guid id = System.Guid.NewGuid();
        private string caption;
        private string tooltipText;
//       protected System.Configuration.Configuration appConfig = System.Configuration.ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Gets or sets the application.
        /// </summary>
        /// <value>The application.</value>
        public DTE2 Application
        {
            get { return application; }
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        public System.Guid Id
        {
            get
            {
                return id;
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return this.GetType().Name; }
        }
        
        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        /// <value>The caption.</value>
        public string Caption
        {
            get
            {
                return caption;
            }
            set
            {
                caption = value;
            }
        }

        /// <summary>
        /// Gets or sets the tooltip text.
        /// </summary>
        /// <value>The tooltip text.</value>
        public string TooltipText
        {
            get
            {
                return tooltipText;
            }
            set
            {
                tooltipText = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CommandBase"/> class.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="caption">The caption code.</param>
        /// <param name="tooltipText">The tooltip text code.</param>
        public CommandBase(DTE2 application, string captionCode, string tooltipTextCode)
        {
            this.application = application;
            this.caption = rm.GetString(captionCode, ci);
            this.tooltipText = rm.GetString(tooltipTextCode, ci);
        }

        /// <summary>
        /// Performs this instance.
        /// </summary>
        public abstract void Perform();
    }
}
