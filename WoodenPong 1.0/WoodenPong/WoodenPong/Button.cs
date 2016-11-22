using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace WoodenPong
{
    public class Button
    {
        public event ChangedEventHandler MouseOver;

        private Game1 _game;
        private Vector2 _position;
        private int _width;
        private int _height;
        private float _opacity;
        private string _text;
        private bool _verif;
        private string _textvar;

        private Texture2D _currentTexture;
        private Texture2D _normal;
        private Texture2D _mouseOver;
        private Texture2D _click;
       

        public Action A;

        public void LoadContent(ContentManager content)
        {
            _normal = content.Load<Texture2D>("IMGS/Button");
            _mouseOver = content.Load<Texture2D>("IMGS/ButtonMouseOver");
            _click = content.Load<Texture2D>("IMGS/ButtonMouseClick");
            _currentTexture = _normal;
        }

        public void Trigger()
        {
            A();
        }

        public Button(Game1 game,Vector2 pos, int width, int height, string text, Action a)
        {
            _game = game;
            _opacity = 0.7f;
            _position = pos;
            _position.X -= (float)width/2;
            _position.Y -= (float)height/2;
            _width = width;
            _height = height;
            _text = text;
            _textvar = null;
            A = a;
        }

        public Button(Game1 game, Vector2 pos, int width, int height, string text, ref string textvar, Action a)
        {
            _game = game;
            _opacity = 0.7f;
            _position = pos;
            _position.X -= (float)width / 2;
            _position.Y -= (float)height / 2;
            _width = width;
            _height = height;
            _text = text;
            _textvar = textvar;
            A = a;
        }

        protected virtual void OnMouseOver(EventArgs e)
        {
            MouseOver?.Invoke(this, e);
        }

        public void Update(GameTime gt)
        {
            if (_textvar != null)
            {
                 _textvar = _game.IsFullScreenString;
            }
           
            if (Mouse.GetState().Position.X >= _position.X && Mouse.GetState().Position.X <= _position.X + _width &&
                Mouse.GetState().Position.Y >= _position.Y
                && Mouse.GetState().Position.Y <= _position.Y + _height &&
                Mouse.GetState().LeftButton == ButtonState.Released && _verif)
            {
                _verif = false;
                Trigger();
            }
            else if (Mouse.GetState().Position.X >= _position.X && Mouse.GetState().Position.X <= _position.X + _width && Mouse.GetState().Position.Y >= _position.Y
                && Mouse.GetState().Position.Y <= _position.Y + _height && Mouse.GetState().LeftButton == ButtonState.Released)
            {
                _currentTexture = _mouseOver;
                _opacity = 0.85f;
            }
            else if (Mouse.GetState().Position.X >= _position.X && Mouse.GetState().Position.X <= _position.X + _width &&
                     Mouse.GetState().Position.Y >= _position.Y
                     && Mouse.GetState().Position.Y <= _position.Y + _height &&
                     Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                _currentTexture = _click;
                _opacity = 1f;
                _verif = true;
            }
            else
            {
                
                _currentTexture = _normal;
                _opacity = 0.7f;
                _verif = false;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(_currentTexture,new Rectangle((int)_position.X, (int)_position.Y, _width, _height), Color.White*_opacity);
            if (_textvar != null)
            {
                sb.DrawString(_game.Font22, _text + _textvar,
                    new Vector2(_position.X + _width/2f - _game.Font22.MeasureString(_text).X/2,
                    _position.Y + _height/2f  - _game.Font22.MeasureString(_text).Y / 2),Color.White*_opacity);
            }
            else
            {
                sb.DrawString(_game.Font22, _text, 
                    new Vector2(_position.X + _width/2f -_game.Font22.MeasureString(_text).X / 2,
                    _position.Y + _height/2f -_game.Font22.MeasureString(_text).Y / 2), Color.White * _opacity);
            }
            
        }
    }
}
