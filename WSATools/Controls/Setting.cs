using System.ComponentModel;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;

namespace WSATools.Controls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WSATools.Controls"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WSATools.Controls;assembly=WSATools.Controls"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:Setting/>
    ///
    /// </summary>
    [TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
    [TemplatePart(Name = RightIconPresenter, Type = typeof(Border))]
    [TemplatePart(Name = PartIconPresenter, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = PartDescriptionPresenter, Type = typeof(ContentPresenter))]
    public class Setting : ContentControl
    {
        private const string PartIconPresenter = "IconPresenter";
        private const string PartDescriptionPresenter = "DescriptionPresenter";
        private const string RightIconPresenter = "ExpandCollapseChevronBorder";
        private Border _rightIconPresenter;
        private ContentPresenter _iconPresenter;
        private ContentPresenter _descriptionPresenter;
        private Setting _setting;

        public Setting()
        {
            this.DefaultStyleKey = typeof(Setting);
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
           "Header",
           typeof(string),
           typeof(Setting),
           new PropertyMetadata(default(string), OnHeaderChanged));

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            "Description",
            typeof(object),
            typeof(Setting),
            new PropertyMetadata(null, OnDescriptionChanged));

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon",
            typeof(object),
            typeof(Setting),
            new PropertyMetadata(default(string), OnIconChanged));

        public static readonly DependencyProperty RightIconProperty = DependencyProperty.Register(
            "RightIcon",
            typeof(string),
            typeof(Setting),
            new PropertyMetadata(null, OnIconChanged));

        public static readonly DependencyProperty ActionContentProperty = DependencyProperty.Register(
            "ActionContent",
            typeof(object),
            typeof(Setting),
            null);

        //public static readonly DependencyProperty RightIconVisibilityProperty = DependencyProperty.Register(
        //    "RightIconVisibility",
        //    typeof(Visibility),
        //    typeof(Setting),
        //    new PropertyMetadata(null, OnIconChanged));

        [Localizable(true)]
        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        [Localizable(true)]
        public object Description
        {
            get => (object)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public object Icon
        {
            get => (object)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public string RightIcon
        {
            get => (string)GetValue(RightIconProperty);
            set => SetValue(RightIconProperty, value);
        }

        public object ActionContent
        {
            get => (object)GetValue(ActionContentProperty);
            set => SetValue(ActionContentProperty, value);
        }

        //public Visibility RightIconVisibility
        //{
        //    get => (Visibility)GetValue(RightIconVisibilityProperty);
        //    set => SetValue(RightIconVisibilityProperty, value);
        //}

        public override void OnApplyTemplate()
        {
            IsEnabledChanged -= Setting_IsEnabledChanged;
            _setting = (Setting)this;
            _rightIconPresenter = (Border)_setting.GetTemplateChild(RightIconPresenter);
            _iconPresenter = (ContentPresenter)_setting.GetTemplateChild(PartIconPresenter);
            _descriptionPresenter = (ContentPresenter)_setting.GetTemplateChild(PartDescriptionPresenter);
            Update();
            SetEnabledState();
            IsEnabledChanged += Setting_IsEnabledChanged;
            base.OnApplyTemplate();
        }

        private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Setting)d).Update();
        }

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Setting)d).Update();
        }

        private static void OnDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Setting)d).Update();
        }

        private void Setting_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SetEnabledState();
        }

        private void SetEnabledState()
        {
            VisualStateManager.GoToState(this, IsEnabled ? "Normal" : "Disabled", true);
        }

        private void Update()
        {
            if (_setting == null)
            {
                return;
            }

            if (_setting.ActionContent != null)
            {
                if (_setting.ActionContent.GetType() != typeof(Button))
                {
                    // We do not want to override the default AutomationProperties.Name of a button. Its Content property already describes what it does.
                    if (!string.IsNullOrEmpty(_setting.Header))
                    {
                        AutomationProperties.SetName((UIElement)_setting.ActionContent, _setting.Header);
                    }
                }
            }

            if (_setting._iconPresenter != null)
            {
                if (_setting.Icon == null)
                {
                    _setting._iconPresenter.Visibility = Visibility.Collapsed;
                }
                else
                {
                    _setting._iconPresenter.Visibility = Visibility.Visible;
                }
            }

            if(_setting.RightIcon == null)
            {
                _setting._rightIconPresenter.Visibility = Visibility.Collapsed;
            }
            else
            {
                _setting._rightIconPresenter.Visibility = Visibility.Visible;
            }

            if (_setting.Description == null)
            {
                _setting._descriptionPresenter.Visibility = Visibility.Collapsed;
            }
            else
            {
                _setting._descriptionPresenter.Visibility = Visibility.Visible;
            }
        }
    }
}
