using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WoodenPong
{
    class SongPlayer
    {
        private readonly Game1 _game;
        private KeyboardState _stdin;
        private bool _isDown;
        private bool _isPause;

        public SongPlayer(Game1 game)
        {
            _game = game;
            MediaPlayer.Volume = 0.1f;
            Song song = game.Content.Load<Song>("SONGS/Veorra & The Tech Thieves Ghost Town");
            MediaPlayer.Play(song);
        }

        public void Update(double time)
        {
            _stdin = Keyboard.GetState();
            if (_game.MusicIsOn)
            {
                if (_game.Pause)
                {
                    MediaPlayer.Pause();
                    _isPause = true;
                }
                else
                {
                    if (!_game.Pause && _isPause)
                    {
                        MediaPlayer.Resume();
                        _isPause = false;
                    }
                    if (_stdin.IsKeyDown(Keys.Add))
                    {
                        MediaPlayer.Volume += 0.5f * (float)time;
                    }

                    if (_stdin.IsKeyDown(Keys.Subtract))
                    {
                        MediaPlayer.Volume -= 0.5f * (float)time;
                    }

                    if (_stdin.IsKeyUp(Keys.Multiply))
                    {
                        _isDown = false;
                    }

                    if (_stdin.IsKeyDown(Keys.Multiply))
                    {
                        if (MediaPlayer.IsMuted)
                        {
                            if (!_isDown)
                            {
                                MediaPlayer.IsMuted = false;
                                _isDown = true;
                            }
                        }
                        else
                        {
                            if (!_isDown)
                            {
                                MediaPlayer.IsMuted = true;
                                _isDown = true;
                            }
                        }
                    }
                }
            }
            
                        
        }
    }
}
