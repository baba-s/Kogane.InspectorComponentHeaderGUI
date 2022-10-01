using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kogane.Internal
{
    [InitializeOnLoad]
    internal static class InspectorComponentHeaderGUI
    {
        private static readonly List<VisualElement> m_containerList = new();

        static InspectorComponentHeaderGUI()
        {
            Selection.selectionChanged      -= Refresh;
            Selection.selectionChanged      += Refresh;
            ObjectFactory.componentWasAdded -= OnComponentWasAdded;
            ObjectFactory.componentWasAdded += OnComponentWasAdded;
            Undo.undoRedoPerformed          -= Refresh;
            Undo.undoRedoPerformed          += Refresh;

            // 2 フレーム遅らせないと Inspector に VisualElement を追加できない
            EditorApplication.delayCall += () =>
                EditorApplication.delayCall += () => Refresh();
        }

        private static void OnComponentWasAdded( Component component )
        {
            EditorApplication.delayCall += () => Refresh();
        }

        private static void Refresh()
        {
            foreach ( var container in m_containerList )
            {
                container.parent.Remove( container );
            }

            m_containerList.Clear();

            if ( !InspectorWindowManager.TryGet( out var inspectorWindow ) ) return;

            var headerElementArray = VisualElementFinder.FindHeaderElementArray( inspectorWindow );

            if ( headerElementArray.Length <= 0 ) return;

            foreach ( var headerElement in headerElementArray )
            {
                var headerElementName = headerElement.name;

                if ( headerElementName == "TransformHeader" ||
                     headerElementName == "Rect TransformHeader" )
                {
                    continue;
                }

                var container = VisualElementCreator.CreateContainer
                (
                    headerElementName: headerElementName,
                    onRefresh: () => EditorApplication.delayCall += () => Refresh()
                );

                headerElement.Add( container );

                m_containerList.Add( container );
            }
        }
    }
}