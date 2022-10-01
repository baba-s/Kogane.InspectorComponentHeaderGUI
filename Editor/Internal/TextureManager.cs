using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Kogane.Internal
{
    internal static class TextureManager
    {
        private static readonly Dictionary<ButtonType, string> GUID_DICTIONARY = new()
        {
            { ButtonType.REMOVE_COMPONENT, "81ba6b20930dc4a4ead95f969e255970" },
            { ButtonType.MOVE_UP, "76ede05262f8bc643aae0c54085298b1" },
            { ButtonType.MOVE_DOWN, "661ee6cb51d809c4abb4f09727ae9c26" },
        };

        private static readonly Dictionary<ButtonType, Texture2D> TEXTURE_DICTIONARY = new();

        public static Texture2D Get( ButtonType buttonType )
        {
            if ( TEXTURE_DICTIONARY.TryGetValue( buttonType, out var result ) ) return result;

            var guid      = GUID_DICTIONARY[ buttonType ];
            var assetPath = AssetDatabase.GUIDToAssetPath( guid );
            var texture   = AssetDatabase.LoadAssetAtPath<Texture2D>( assetPath );

            TEXTURE_DICTIONARY[ buttonType ] = texture;

            return texture;
        }
    }
}