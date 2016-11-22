using System;

namespace WoodenPong
{
    class ButtonEventListener
    {
        private Button _buttonToListen;

        public ButtonEventListener(Button b)
        {
            _buttonToListen = b;
            b.MouseOver += EventFire;
        }

        private void EventFire(object sender, EventArgs e)
        {

        }

        public void Detach()
        {
            // Detach the event and delete the list
            _buttonToListen.MouseOver -= EventFire;
            _buttonToListen = null;
        }
    }
}
