using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

namespace Kogane.Internal
{
    internal static class VisualElementFinder
    {
        public static VisualElement[] FindHeaderElementArray( EditorWindow inspectorWindow)
        {
            // ")Header" の場合はスクリプトの Inspector のヘッダーなので無視
            // 先頭の要素は Transform のヘッダーなので無視
            return inspectorWindow.rootVisualElement
                    .Query<VisualElement>()
                    .Where( x => x.name.EndsWith( "Header" ) && !x.name.EndsWith( ")Header" ) )
                    .ToList()
                    .Skip( 1 )
                    .ToArray()
                ;
        }
    }
}