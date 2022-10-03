using System;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

namespace Kogane.Internal
{
    internal static class VisualElementFinder
    {
        public static VisualElement[] FindHeaderElementArray( EditorWindow inspectorWindow )
        {
            var rootVisualElement = inspectorWindow.rootVisualElement;
            var visualElementList = rootVisualElement.Query<VisualElement>().ToList();

            // Project ウィンドウでプレハブを選択した時は無視
            if ( visualElementList.Any( x => x.name == "Prefab ImporterHeader" ) )
            {
                return Array.Empty<VisualElement>();
            }

            // 先頭の要素は Transform のヘッダーなので無視
            return visualElementList
                    .Where
                    (
                        x => !x.name.StartsWith( "Inspector Component Header GUI" ) &&
                             ( ( x.name.EndsWith( "Header" ) && !x.name.EndsWith( ")Header" ) ) || x.name.EndsWith( "(Script)Header" ) )
                    )
                    .ToList()
                    .Skip( 1 )
                    .ToArray()
                ;
        }
    }
}