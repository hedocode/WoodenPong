using System;

namespace WoodenPong
{
    class InputsEventListener
    {
        private Inputs _inputsToListen;

        public InputsEventListener(Inputs inputs)
        {
            _inputsToListen = inputs;
            inputs.Changed += EventFire;
        }

        private void EventFire(object sender, EventArgs e)
        {

        }

        public void Detach()
        {
            // Detach the event and delete the list
            _inputsToListen.Changed -= EventFire;
            _inputsToListen = null;
        }
    }
}
