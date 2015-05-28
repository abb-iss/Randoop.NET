using System;
using System.IO;
using EnvDTE;
using EnvDTE80;
using Extensibility;
using Microsoft.VisualStudio.CommandBars;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.Collections.Generic;
using System.Windows.Forms;
//using log4net;

namespace Randoop
{
    public class MenuManager
    {
        #region MEMBER VARIABLES
        ///////////////////////////////////////////////////////////////////////
        //
        // MEMBER VARIABLES
        //
        ///////////////////////////////////////////////////////////////////////
        //protected static readonly ILog _logger = LogManager.GetLogger((MethodBase.GetCurrentMethod().DeclaringType));
        private DTE2 application;
        private List<CommandBarEvents> menuItemHandlerList = new List<CommandBarEvents>();
        private Dictionary<string, CommandBase> cmdList = new Dictionary<string, CommandBase>();
        #endregion

        #region INITIALIZATION
        ///////////////////////////////////////////////////////////////////////
        //
        // INITIALIZATION
        //
        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MenuManager"/> class.
        /// </summary>
        /// <param name="application">The application.</param>
        public MenuManager(DTE2 application)
        {
            this.application = application;
        }

        #endregion

        #region MENU CREATION
        ///////////////////////////////////////////////////////////////////////
        //
        // MENU CREATION
        //
        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates the popup menu.
        /// </summary>
        /// <param name="commandBarName">Name of the command bar.</param>
        /// <param name="menuName">Name of the menu.</param>
        /// <returns></returns>
        public CommandBarPopup CreatePopupMenu(string commandBarName, string menuName)
        {
            if (GetCommandBar(commandBarName) != null)
            {
                CommandBarPopup menu = GetCommandBar(commandBarName).Controls.Add(MsoControlType.msoControlPopup, Missing.Value, Missing.Value, 1, true) as CommandBarPopup;
                menu.Caption = menuName;
                menu.TooltipText = "";
                return menu;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Creates the popup menu.
        /// </summary>
        /// <param name="popupMenu">The popup menu.</param>
        /// <param name="subPopupMenuName">Name of the sub popup menu.</param>
        /// <param name="position">The position.</param>
        public CommandBarPopup CreatePopupMenu(CommandBarPopup popupMenu, string subPopupMenuName, int position)
        {
            CommandBarPopup menu = (CommandBarPopup)popupMenu.Controls.Add(MsoControlType.msoControlPopup, 1, "", position, true);
            menu.Caption = subPopupMenuName;
            menu.TooltipText = "";
            return menu;
        }

        /// <summary>
        /// Gets the command bar.
        /// </summary>
        /// <param name="commandBarName">Name of the command bar.</param>
        /// <returns></returns>
        private CommandBar GetCommandBar(string commandBarName)
        {
            try
            {
                return ((CommandBars)application.DTE.CommandBars)[commandBarName];
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region MENU ITEMS CREATION
        ///////////////////////////////////////////////////////////////////////
        //
        // MENU ITEMS CREATION
        //
        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds the command.
        /// </summary>
        /// <param name="menu">The popup menu.</param>
        /// <param name="cmd">The command.</param>
        /// <param name="position">The position in the menu.</param>
        public void AddCommandMenu(CommandBarPopup popupMenu, CommandBase cmd, int position)
        {
            CommandBarControl menuItem = AddMenuItem(popupMenu, cmd, position);
            AddClickEventHandler(menuItem);
            AddCommandToList(cmd);
        }
    
        /// <summary>
        /// Add the menu item to the popup menu
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="cmd">The CMD.</param>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        private CommandBarControl AddMenuItem(CommandBarPopup menu, CommandBase cmd, int position)
        {
            CommandBarControl menuItem = menu.Controls.Add(MsoControlType.msoControlButton, 1, "", position, true);
            menuItem.Tag = cmd.Id.ToString();
            menuItem.Caption = cmd.Caption;
            menuItem.TooltipText = cmd.TooltipText;
            return menuItem;
        }

        /// <summary>
        /// Adds handler to the menu item click event.
        /// </summary>
        /// <param name="menuItem">The menu item.</param>
        private void AddClickEventHandler(CommandBarControl menuItem)
        {
            CommandBarEvents menuItemHandler = (EnvDTE.CommandBarEvents)application.DTE.Events.get_CommandBarEvents(menuItem);
            menuItemHandler.Click += new _dispCommandBarControlEvents_ClickEventHandler(MenuItem_Click);
            menuItemHandlerList.Add(menuItemHandler);
        }

        /// <summary>
        /// Adds the command to list.
        /// </summary>
        /// <param name="cmd">The CMD.</param>
        private void AddCommandToList(CommandBase cmd)
        {
            if (!cmdList.ContainsKey(cmd.Id.ToString()))
            {
                cmdList.Add(cmd.Id.ToString(), cmd);
            }
        }
        #endregion

        #region MENU ITEMS HANDLER
        ///////////////////////////////////////////////////////////////////////
        //
        // MENU ITEMS HANDLER
        //
        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Handles the click on the menu item
        /// </summary>
        /// <param name="commandBarControl">The command bar control.</param>
        /// <param name="handled">if set to <c>true</c> [handled].</param>
        /// <param name="cancelDefault">if set to <c>true</c> [cancel default].</param>
        private void MenuItem_Click(object commandBarControl, ref bool handled, ref bool cancelDefault)
        {
            try
            {
                // We perform the command only if we found the command corresponding to the menu item clicked
                CommandBarControl menuItem = (CommandBarControl)commandBarControl;
                if (cmdList.ContainsKey(menuItem.Tag))
                {
                    cmdList[menuItem.Tag].Perform();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            //catch (ExplorerException e)
            //{
            //    _logger.ErrorFormat("An error occured : {0}", e.Message);
            //    MessageBox.Show(e.Message, "Explorer add in");
            //}
        }
        #endregion
    }
}
