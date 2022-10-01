using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Kogane.Internal
{
    internal static class InspectorWindowManager
    {
        private static readonly Type INSPECTOR_WINDOW_TYPE =
            typeof( Editor ).Assembly.GetType( "UnityEditor.InspectorWindow" );

        private static EditorWindow m_inspectorWindow;

        public static bool TryGet( out EditorWindow inspectorWindow )
        {
            if ( m_inspectorWindow != null )
            {
                inspectorWindow = m_inspectorWindow;
                return true;
            }

            m_inspectorWindow = ( EditorWindow )Resources
                    .FindObjectsOfTypeAll( INSPECTOR_WINDOW_TYPE )
                    .FirstOrDefault()
                ;

            inspectorWindow = m_inspectorWindow;

            return m_inspectorWindow != null;
        }
    }
}