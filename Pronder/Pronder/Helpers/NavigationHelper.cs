using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;

namespace Pronder.Helpers;

public static class NavigationHelper
{
    public static void SetNavigateTo(DependencyObject obj, Type value) => obj.SetValue(NavigateToProperty, value);
    public static Type GetNavigateTo(DependencyObject obj) => (Type)obj.GetValue(NavigateToProperty);
    public static void SetFrame(DependencyObject obj, Frame value) => obj.SetValue(FrameProperty, value);
    public static Frame GetFrame(DependencyObject obj) => (Frame)obj.GetValue(FrameProperty);
    
    public static readonly DependencyProperty FrameProperty = DependencyProperty.RegisterAttached(
            "Frame",
            typeof(Frame),
            typeof(NavigationHelper),
            new PropertyMetadata(null)
    );
    
    public static readonly DependencyProperty NavigateToProperty = DependencyProperty.RegisterAttached(
            "NavigateTo",
            typeof(Type),
            typeof(NavigationHelper),
            new PropertyMetadata(null, OnNavigateToChanged)
    );
    
    private static void OnNavigateToChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is MenuFlyoutItem menuItem && e.NewValue is Type pageType)
        {
            menuItem.Click -= MenuItem_Click;
            menuItem.Click += MenuItem_Click;
        }
    }
    
    private static void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is MenuFlyoutItem menuItem)
        {
            var pageType = GetNavigateTo(menuItem);
            var frame = GetFrame(menuItem);
            
            if ((pageType != null && frame != null) && (frame.CurrentSourcePageType != pageType))
                frame.Navigate(pageType, null, new DrillInNavigationTransitionInfo());
        }
    }
}