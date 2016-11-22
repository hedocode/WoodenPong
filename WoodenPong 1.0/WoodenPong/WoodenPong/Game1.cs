using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace WoodenPong
{
    /// <summary>
    ///     This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public enum GameStates
        {
            Intro,
            Game,
            GameOver,
            Credits
        }

        private readonly GraphicsDeviceManager _graphics;
        private readonly Inputs _inputs;
        private readonly SongPlayer _sp;
        private SpriteBatch _spriteBatch;

        public bool DisplayOptions;
        private bool _decOpa;
        public bool Pause;
        public bool SoundIsOn;
        public bool MusicIsOn;

        private Color _backgroundColor;
        private Texture2D _background;
        private Texture2D _ballTexture;

        public SpriteFont Font12;
        public SpriteFont Font14;
        public SpriteFont Font16;
        public SpriteFont Font18;
        public SpriteFont Font20;
        public SpriteFont Font22;
        public SpriteFont Font24;
        public SpriteFont Font26;

        public GameStates GameState;
        private string _gameStateTitle;

        private Texture2D _logo;
        public string IsFullScreenString;
        private string _message1;
        private string _message2;
        private float _opacity;
        private float _opacity2;

        public SoundEffect BallHitLeft { get; private set; }
        public SoundEffect BallHitUpDown { get; private set; }
        public SoundEffect BallHitRight { get; private set; }
        //private SoundEffectInstance instance;


        public int WindowsHeight { get; }
        public int WindowsWidth { get; }

        public int Scorep1 { get; set; }
        public int Scorep2 { get; set; }

        public Paddle R1 { get; private set; }
        public Paddle R2 { get; private set; }
        public Ball Ball { get; private set; }
        public List<Button> ButtonList;

        /// Game Constructor
        public Game1()
        {
            WindowsWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            WindowsHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Pause = false;

            ButtonList = new List<Button>();

            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = WindowsHeight,
                PreferredBackBufferWidth = WindowsWidth,
                IsFullScreen = false
            };
            IsMouseVisible = true;
            Content.RootDirectory = "Content";

            R1 = new Paddle(this, false);
            R2 = new Paddle(this, true);

            _inputs = new Inputs(this);
            _sp = new SongPlayer(this);

            IsFullScreenString = _graphics.IsFullScreen.ToString();

            ButtonList.Add(new Button(this, new Vector2((float)WindowsWidth / 2, WindowsHeight / 1.5f), WindowsWidth / 6, WindowsHeight / 12, "EXIT", Exit));
            ButtonList.Add(new Button(this, new Vector2((float)WindowsWidth / 2 , (float)WindowsHeight / 4 ), WindowsWidth / 6, WindowsHeight / 12, "RESUME", LeaveOptions));
            ButtonList.Add(new Button(this, new Vector2((float)WindowsWidth / 2, (float)WindowsHeight / 2), WindowsWidth / 6, WindowsHeight / 12, "Fullscreen :", ref IsFullScreenString, ScreenState));
            ButtonList.Add(new Button(this, new Vector2((float)WindowsWidth / 2, (float)WindowsHeight / 3), WindowsWidth / 6, WindowsHeight / 12, "ToogleSound", ToogleSound));
            ButtonList.Add(new Button(this, new Vector2((float)WindowsWidth / 2, WindowsHeight / 2.5f), WindowsWidth / 6, WindowsHeight / 12, "ToogleMusic", ToogleMusic));

            _opacity = 0;
        }

        public void ToogleMusic()
        {
            MusicIsOn = !MusicIsOn;
        }

        public void ToogleSound()
        {
            SoundIsOn = !SoundIsOn;
        }

        public void ScreenState()
        {
            _graphics.ToggleFullScreen();
        }

        public void LeaveOptions()
        {
            _inputs.EscIsDown = false;
            Pause = false;
            DisplayOptions = false;
        }


        /// <summary>
        ///     Allows the game to perform any initialization it needs to before starting to run.
        ///     This is where it can query for any required services and load any non-graphic
        ///     related content.  Calling base.Initialize will enumerate through any components
        ///     and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _backgroundColor = Color.DeepSkyBlue;
            GameState = GameStates.Intro;
            _gameStateTitle = " Intro ";
            SoundIsOn = true;
            Scorep1 = 0;
            Scorep2 = 0;
            _message1 = "Player 1 : " + Scorep1;
            _message2 = "Player 2 : " + Scorep2;
            base.Initialize();
        }

        /// <summary>
        ///     LoadContent will be called once per game and is the place to load
        ///     all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _background = Content.Load<Texture2D>("IMGS/bg");
            _ballTexture = Content.Load<Texture2D>("IMGS/ball");
            _logo = Content.Load<Texture2D>("IMGS/Logo");
            Font12 = Content.Load<SpriteFont>("FONTS/font12");
            Font14 = Content.Load<SpriteFont>("FONTS/font14");
            Font16 = Content.Load<SpriteFont>("FONTS/font16");
            Font18 = Content.Load<SpriteFont>("FONTS/font18");
            Font20 = Content.Load<SpriteFont>("FONTS/font20");
            Font22 = Content.Load<SpriteFont>("FONTS/font22");
            Font24 = Content.Load<SpriteFont>("FONTS/font24");
            Font26 = Content.Load<SpriteFont>("FONTS/font26");


            BallHitLeft = Content.Load<SoundEffect>("ping_left");
            BallHitRight = Content.Load<SoundEffect>("ping_right");
            BallHitUpDown = Content.Load<SoundEffect>("ping");

            R1.LoadContent(Content);
            R2.LoadContent(Content);

            foreach (Button b in ButtonList)
            {
                b.LoadContent(Content);
            }

            //instance = ball_hit_left.CreateInstance();
            Ball = new Ball(this, R2, _ballTexture);
        }

        /// <summary>
        ///     UnloadContent will be called once per game and is the place to unload
        ///     game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            _background.Dispose();
            R1 = null;
            R2 = null;
        }

        /// <summary>
        ///     Allows the game to run logic such as updating the world,
        ///     checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            //Inputs Updates
            _inputs.Update(gameTime);

            // GameState Updates
            if (GameState == GameStates.Intro)
            {
                if (gameTime.TotalGameTime.Seconds < 3)
                {
                    _opacity2 += 0.5f*(float) gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    if (_opacity <= 0.2)
                        _decOpa = false;
                    if (_opacity >= 1)
                        _decOpa = true;

                    if (_decOpa)
                        _opacity -= 0.6f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    else
                        _opacity += 0.6f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                
                
            }
            if (GameState == GameStates.Game)
            {
                IsFullScreenString = _graphics.IsFullScreen.ToString();
                _gameStateTitle = "Game";
                //Sounds Updates

                if (MusicIsOn)
                    _sp.Update(gameTime.ElapsedGameTime.TotalSeconds);

                if (DisplayOptions)
                {
                    foreach (Button b in ButtonList)
                    {
                        b.Update(gameTime);
                    }
                }

                if (!Pause)
                {
                    //Collisions Updates
                    Vector2 ballspeed = Ball.Velocity;
                    Vector2 ballposition = Ball.Position;
                    float a = ballspeed.Y/ballspeed.X;
                    float b = ballposition.Y - a * ballposition.X; 

                    Ball bafter = new Ball(this, (int)(ballposition.X+ballspeed.X*gameTime.ElapsedGameTime.TotalSeconds), 
                        (int)(ballposition.Y+ballspeed.Y * gameTime.ElapsedGameTime.TotalSeconds), _ballTexture,Vector2.Zero);

                    /*
                    foreach (Paddle p in paddleList)
                    {
                        if (p.IsRight)
                        {

                        }
                        else
                        {
                            
                        }
                    }
                    */


                    //WIP
                    if (a*(R1.Position.X + R1.Width) + b > R1.Position.Y - Ball.Height && a*(R1.Position.X + R1.Width) + b < R1.Position.Y + R1.Height 
                        && Ball.Position.X > R1.Position.X+R1.Width && bafter.Position.X <= R1.Position.X+R1.Width)
                    {
                        Vector2 v1 = Ball.Velocity;
                        v1.X = -11 * v1.X / 10;
                        Ball.Velocity = v1;
                        v1 = new Vector2(Ball.Velocity.X, Ball.Velocity.Y + R1.Velocity.Y / 2);
                        Ball.Velocity = v1;
                        if (SoundIsOn)
                            BallHitLeft.Play();
                    }
                    else if ( (a * R1.Position.X + b > R1.Position.Y && a * (R1.Position.X+R1.Width) + b < R1.Position.Y &&
                        ballposition.Y + Ball.Height < R1.Position.Y && bafter.Position.Y + Ball.Height > R1.Position.Y) || 
                        (a * R1.Position.X + b < R1.Position.Y + R1.Height && a * (R1.Position.X + R1.Width) + b > R1.Position.Y + R1.Height &&
                        ballposition.Y > R1.Position.Y+R1.Height && bafter.Position.Y <= R1.Position.Y+R1.Height ))
                    {
                        Vector2 v1 = Ball.Velocity;
                        v1.Y = -11 * v1.Y / 10;
                        Ball.Velocity = v1;
                        v1 = new Vector2(Ball.Velocity.X, Ball.Velocity.Y + R1.Velocity.Y / 2);
                        Ball.Velocity = v1;
                        if (SoundIsOn)
                            BallHitLeft.Play();
                    }

                    if (a * R2.Position.X + b + Ball.Width > R2.Position.Y && a * R2.Position.X + b + Ball.Width < R2.Position.Y + R2.Height + Ball.Height && 
                        Ball.Position.X + Ball.Width < R2.Position.X  && bafter.Position.X + bafter.Width >= R2.Position.X)
                    {
                        Vector2 v1 = Ball.Velocity;
                        v1.X = -11 * v1.X / 10;
                        Ball.Velocity = v1;
                        v1 = new Vector2(Ball.Velocity.X, Ball.Velocity.Y + R2.Velocity.Y / 2);
                        Ball.Velocity = v1;
                        if (SoundIsOn)
                            BallHitLeft.Play();
                    }
                    else if ((a * (R2.Position.X+R2.Width) + b < R2.Position.Y && a * R2.Position.X + b > R2.Position.Y &&
                        ballposition.Y + Ball.Height < R2.Position.Y && bafter.Position.Y + Ball.Height > R2.Position.Y) ||
                        (a * (R2.Position.X+R2.Width) + b > R2.Position.Y + R2.Height &&
                        a * R2.Position.X  + b < R2.Position.Y + R2.Height &&
                        ballposition.Y >= R2.Position.Y + R2.Height && bafter.Position.Y < R2.Position.Y + R2.Height))
                    {
                        Vector2 v1 = Ball.Velocity;
                        v1.Y = -11 * v1.Y / 10;
                        Ball.Velocity = v1;
                        v1 = new Vector2(Ball.Velocity.X, Ball.Velocity.Y + R2.Velocity.Y / 2);
                        Ball.Velocity = v1;
                        if (SoundIsOn)
                            BallHitLeft.Play();
                    }

                    /*
                    if (Ball.BoundingBox.Intersects(R1.BoundingBox) || Ball.BoundingBox.Intersects(R2.BoundingBox))
                    {
                        if ((Ball.Position.X >= R1.Position.X + R1.Width + Ball.Velocity.X * gameTime.ElapsedGameTime.TotalSeconds) && (Ball.Position.X <= R2.Position.X - Ball.Width + Ball.Velocity.X*gameTime.ElapsedGameTime.TotalSeconds)
                            /*&& ball.Position.Y > r1.Position.Y && ball.Position.Y < r1.Position.Y + r1.Height)
                        {
                            Vector2 v1 = Ball.Velocity;
                            v1.X = -11*v1.X/10;
                            Ball.Velocity = v1;
                        }
                        else
                        {
                            if (!Ball.HasHitAntibug)
                            {
                                Vector2 v1 = Ball.Velocity;
                                v1.Y = -v1.Y;
                                Ball.Velocity = v1;
                                if (Ball.BoundingBox.Intersects(R1.BoundingBox))
                                {
                                    Vector2 v2 = new Vector2(Ball.Velocity.X, -Ball.Velocity.Y + R1.Velocity.Y);
                                    Ball.Velocity = v2;
                                    Ball.HasHitAntibug = true;
                                }
                                if (Ball.BoundingBox.Intersects(R2.BoundingBox))
                                {
                                    Vector2 v2 = new Vector2(Ball.Velocity.X, -Ball.Velocity.Y + R2.Velocity.Y);
                                    Ball.Velocity = v2;
                                    Ball.HasHitAntibug = true;
                                }
                            }
                        }

                        if (Ball.BoundingBox.Intersects(R1.BoundingBox))
                        {
                            Vector2 v1 = new Vector2(Ball.Velocity.X, Ball.Velocity.Y + R1.Velocity.Y/2);
                            Ball.Velocity = v1;
                            if (SoundIsOn)
                                BallHitLeft.Play();
                        }
                        if (Ball.BoundingBox.Intersects(R2.BoundingBox))
                        {
                            Vector2 v1 = new Vector2(Ball.Velocity.X, Ball.Velocity.Y + R2.Velocity.Y/2);
                            Ball.Velocity = v1;
                            if (SoundIsOn)
                                BallHitRight.Play();
                        }
                    }
                    */

                    // Sprites Updates
                    R1.Update(gameTime.ElapsedGameTime.TotalSeconds);
                    R2.Update(gameTime.ElapsedGameTime.TotalSeconds);
                    Ball.Update(gameTime.ElapsedGameTime.TotalSeconds);

                    //Others Updates
                    _message1 = "Player 1 : " + Scorep1;
                    _message2 = "Player 2 : " + Scorep2;
                    R1.Velocity = new Vector2(0, 0);
                    R2.Velocity = new Vector2(0, 0);
                }
            }
            if (GameState == GameStates.GameOver)
            {
            }


            base.Update(gameTime);
        }


        /// <summary>
        ///     This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(_backgroundColor);

            // TODO: Add your drawing code here

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

            if (GameState == GameStates.Intro)
            {
                if (gameTime.TotalGameTime.Seconds < 3)
                {
                    _spriteBatch.Draw(_logo, new Rectangle(WindowsWidth/2 - WindowsHeight/4, WindowsHeight/10, WindowsHeight/2, WindowsHeight/2), Color.White*_opacity2);
                }
                else
                {
                    _spriteBatch.Draw(_logo, new Rectangle(WindowsWidth/2 - WindowsHeight/4, WindowsHeight/10, WindowsHeight/2,WindowsHeight/2), Color.White);
                    
                    const string pressEnter = "PRESS ENTER TO PLAY !";
                    _spriteBatch.DrawString(Font12, pressEnter, new Vector2(WindowsWidth/2 - pressEnter.Length*7, (float)9*WindowsHeight/10), Color.White*_opacity);
                }
            }
            else if (GameState == GameStates.Game)
            {
                _spriteBatch.Draw(_background, new Rectangle(0, 0, WindowsWidth, WindowsHeight), Color.White);
                R1?.Draw(_spriteBatch);
                R2?.Draw(_spriteBatch);
                Ball?.Draw(_spriteBatch);
                _spriteBatch.DrawString(Font12, _message1, new Vector2(WindowsWidth/4 - 50, (float)WindowsHeight/100), Color.White);
                _spriteBatch.DrawString(Font12, _message2, new Vector2(3*WindowsWidth/4 - 50, (float)WindowsHeight/100), Color.White);

                if (DisplayOptions)
                {
                    Texture2D x1 = new Texture2D(_graphics.GraphicsDevice, 1, 1);
                    x1.SetData(new[] {Color.White});
                    _spriteBatch.Draw(x1,new Rectangle(0,0,WindowsWidth,WindowsHeight),Color.Black*0.5f);
                    foreach (Button b in ButtonList)
                    {
                        b.Draw(_spriteBatch);
                    }
                }
            }

            _spriteBatch.DrawString(Font12, _gameStateTitle, new Vector2(10, 10), Color.White);
            _spriteBatch.DrawString(Font12, Pause.ToString(), new Vector2(200, 10), Color.White);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}