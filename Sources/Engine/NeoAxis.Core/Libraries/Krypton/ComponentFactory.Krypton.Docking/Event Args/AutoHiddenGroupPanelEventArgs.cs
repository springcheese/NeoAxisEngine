#if !DEPLOY
// *****************************************************************************
// 
//  © Component Factory Pty Ltd 2012. All rights reserved.
//	The software and associated documentation supplied hereunder are the 
//  proprietary information of Component Factory Pty Ltd, 17/267 Nepean Hwy, 
//  Seaford, Vic 3198, Australia and are supplied subject to licence terms.
// 
//
// *****************************************************************************

using System;
using System.Drawing;
using System.Diagnostics;
using Internal.ComponentFactory.Krypton.Toolkit;
using Internal.ComponentFactory.Krypton.Navigator;
using Internal.ComponentFactory.Krypton.Workspace;

namespace Internal.ComponentFactory.Krypton.Docking
{
	/// <summary>
    /// Event arguments for a AutoHiddenGroupPanelAdding/AutoHiddenGroupPanelRemoved events.
	/// </summary>
	public class AutoHiddenGroupPanelEventArgs : EventArgs
	{
		#region Instance Fields
        private KryptonAutoHiddenPanel _autoHiddenPanel;
        private KryptonDockingEdgeAutoHidden _element;
		#endregion

		#region Identity
		/// <summary>
        /// Initialize a new instance of the AutoHiddenGroupPanelEventArgs class.
		/// </summary>
        /// <param name="autoHiddenPanel">Reference to auto hidden panel control instance.</param>
        /// <param name="element">Reference to docking auto hidden edge element that is managing the panel.</param>
        public AutoHiddenGroupPanelEventArgs(KryptonAutoHiddenPanel autoHiddenPanel,
                                             KryptonDockingEdgeAutoHidden element)
		{
            _autoHiddenPanel = autoHiddenPanel;
            _element = element;
		}
		#endregion

		#region Public
        /// <summary>
        /// Gets a reference to the KryptonAutoHiddenPanel control.
        /// </summary>
        public KryptonAutoHiddenPanel AutoHiddenPanelControl
        {
            get { return _autoHiddenPanel; }
        }

        /// <summary>
        /// Gets a reference to the KryptonDockingEdgeAutoHidden that is managing the edge.
        /// </summary>
        public KryptonDockingEdgeAutoHidden EdgeAutoHiddenElement
        {
            get { return _element; }
        }
        #endregion
	}
}

#endif