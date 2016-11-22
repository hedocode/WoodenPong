using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace WoodenPong
{
    public delegate void ChangedEventHandler(object sender, EventArgs e);

    internal class Inputs
    {
        private readonly Game1 _game;
        private KeyboardState _stdin;
        private bool _pIsDown;
        public bool EscIsDown;

        public event ChangedEventHandler Changed;

        protected virtual void OnChanged(EventArgs e)
        {
            Changed?.Invoke(this, e);
        }

        public Inputs(Game1 game)
        {
            _game = game;
            _stdin = new KeyboardState();
        }

        public void Update(GameTime gt)
        {
            _stdin = Keyboard.GetState();
            OnChanged(EventArgs.Empty);

            if(_game.GameState == Game1.GameStates.Intro)
            {
                if (gt.TotalGameTime.Seconds > 3 && _stdin.IsKeyDown(Keys.Enter))
                {
                    _game.GameState = Game1.GameStates.Game;
                }
            }

            if (_game.GameState == Game1.GameStates.Game)
            {
                if (!_game.Pause)
                {
                    if (_stdin.IsKeyDown(Keys.Z))
                    {
                        Vector2 v1 = new Vector2(0, (float)-_game.WindowsHeight / 2);
                        _game.R1.Velocity += v1;
                    }
                    if (_stdin.IsKeyDown(Keys.S))
                    {
                        Vector2 v1 = new Vector2(0, (float)_game.WindowsHeight / 2);
                        _game.R1.Velocity += v1;
                    }

                    if (_stdin.IsKeyDown(Keys.Up))
                    {
                        Vector2 v1 = new Vector2(0, (float)-_game.WindowsHeight / 2);
                        _game.R2.Velocity += v1;
                    }
                    if (_stdin.IsKeyDown(Keys.Down))
                    {
                        Vector2 v1 = new Vector2(0, (float)_game.WindowsHeight / 2);
                        _game.R2.Velocity += v1;
                    }

                    if (_stdin.IsKeyDown(Keys.Space) && _game.Ball.IsAttached)
                    {
                        if (_game.Ball.IsAttachedToPaddle(_game.R1))
                        {
                            _game.Ball.Shoot(_game.R1);
                        }
                    }

                    if (_stdin.IsKeyDown(Keys.RightControl) && _game.Ball.IsAttached)
                    {
                        if (_game.Ball.IsAttachedToPaddle(_game.R2))
                        {
                            _game.Ball.Shoot(_game.R2);
                        }
                    }
                }

                if (_stdin.IsKeyUp(Keys.P))
                {
                    _pIsDown = false;
                }
                else
                {
                    if (_game.Pause)
                    {
                        if (!_pIsDown)
                        {
                            _game.Pause = false;
                            _pIsDown = true;
                        }
                    }
                    else
                    {
                        if (!_pIsDown)
                        {
                            _game.Pause = true;
                            _pIsDown = true;
                        }
                    }
                }

                if (_stdin.IsKeyUp(Keys.Escape))
                {
                    EscIsDown = false;
                }
                else
                {
                    if (!EscIsDown)
                    {
                        if (_game.Pause)
                        {
                            _game.Pause = false;
                            _game.DisplayOptions = false;
                        }
                        else
                        {
                            _game.Pause = true;
                            _game.DisplayOptions = true;
                        }
                        EscIsDown = true;
                    }
                    
                }
                
            }
            
        }
    }
}
