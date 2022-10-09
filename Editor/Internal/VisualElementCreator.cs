using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kogane.Internal
{
    internal static class VisualElementCreator
    {
        public static VisualElement CreateContainer( string headerElementName, Action onRefresh )
        {
            var container = new VisualElement
            {
                pickingMode = PickingMode.Ignore,
                style =
                {
                    flexDirection = FlexDirection.RowReverse,
                    height        = 22,
                }
            };

            var imageCreator         = new ImageCreator( headerElementName, onRefresh );
            var removeComponentImage = imageCreator.CreateImage( ButtonType.REMOVE_COMPONENT, x => Undo.DestroyObjectImmediate( x ) );
            var moveUpImage          = imageCreator.CreateImage( ButtonType.MOVE_UP, x => ComponentUtility.MoveComponentUp( x ) );
            var moveDownImage        = imageCreator.CreateImage( ButtonType.MOVE_DOWN, x => ComponentUtility.MoveComponentDown( x ) );
            var copyComponentImage   = imageCreator.CreateImage( ButtonType.COPY_COMPONENT, x => CopyComponent( x ) );

            container.Add( removeComponentImage );
            container.Add( moveUpImage );
            container.Add( moveDownImage );
            container.Add( copyComponentImage );

            return container;
        }

        private static void CopyComponent( Component component )
        {
            ComponentUtility.CopyComponent( component );
            Debug.Log( $"Copied! '{component.GetType().Name}'" );
            TooltipWindow.Open( "Copied!" );
        }

        private sealed class ImageCreator
        {
            private readonly string m_headerElementName;
            private readonly Action m_onRefresh;

            public ImageCreator( string headerElementName, Action onRefresh )
            {
                m_headerElementName = headerElementName;
                m_onRefresh         = onRefresh;
            }

            public Image CreateImage
            (
                ButtonType        buttonType,
                Action<Component> action
            )
            {
                var image = new Image
                {
                    style =
                    {
                        position        = Position.Relative,
                        backgroundImage = TextureManager.Get( buttonType ),
                        top             = 3,
                        right           = 63 + ( int )buttonType * 3,
                        width           = 16,
                        height          = 16,
                    }
                };

                image.RegisterCallback<ClickEvent>
                (
                    _ =>
                    {
                        var componentName = m_headerElementName
                                .Remove( m_headerElementName.Length - 6, 6 )
                                .Replace( " ", "" )
                                .Replace( "(Script)", "" )
                            ;

                        foreach ( var gameObject in Selection.gameObjects )
                        {
                            var component = gameObject.GetComponent( componentName );

                            action( component );
                        }

                        m_onRefresh();
                    }
                );

                return image;
            }
        }
    }
}