using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Wordki.Controls {
  public class TabControlMenu : TabControl {

    private  Storyboard _showStoryboard;
    private  Storyboard _hideStoryboard;
    private bool _showCompleted;
    private bool _hideCompleted;

    public TabControlMenu() {
      _showStoryboard = FindResource("SlideInMenuStoryboard") as Storyboard;
      _hideStoryboard = FindResource("SlideOutMenuStoryboard") as Storyboard;

      _showCompleted = false;
      _hideCompleted = false;
    }

    protected override void OnMouseEnter(MouseEventArgs e) {
      base.OnMouseEnter(e);
      if (_showStoryboard != null) {
        Storyboard.SetTarget(_showStoryboard, this);
        _showStoryboard.Completed += delegate { _showCompleted = true; };
        _showStoryboard.Begin();
      }
    }

    protected override void OnMouseLeave(MouseEventArgs e) {
      base.OnMouseLeave(e);
      _hideStoryboard.Begin();
      if (_hideStoryboard != null) {
        Storyboard.SetTarget(_hideStoryboard, this);
        _hideStoryboard.Completed += delegate { _hideCompleted = true; };
      }
    }
  }
}
