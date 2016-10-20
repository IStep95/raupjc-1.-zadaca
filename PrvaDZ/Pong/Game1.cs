using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Zadatak3;

namespace Pong
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Paddle PaddleBottom { get; private set; }
        public Paddle PaddleTop { get; private set; }
        public Ball Ball { get; private set; }
        public Background Background { get; private set; }
        public SoundEffect HitSound { get; private set; }
        public Song Music { get; private set; }
        

        private IGenericList<Sprite> SpritesForDrawList = new GenericList<Sprite>();
        public List<Wall> Walls { get; set; }
        public List<Wall> Goals { get; set; }


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 900,
                PreferredBackBufferWidth = 500
            };

            Content.RootDirectory = "Content";
           
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            // Screen bounds details. Use this information to set up game objects positions.
            var screenBounds = GraphicsDevice.Viewport.Bounds;

            PaddleBottom = new Paddle(GameConstants.PaddleDefaultWidth, GameConstants.PaddleDefaultHeight, GameConstants.PaddleDefaultSpeed);
            PaddleBottom.X = (screenBounds.Width / 2) - (PaddleBottom.Width / 2);
            PaddleBottom.Y = screenBounds.Height - PaddleBottom.Height;


            PaddleTop = new Paddle(GameConstants.PaddleDefaultWidth, GameConstants.PaddleDefaultHeight, GameConstants.PaddleDefaultSpeed);
            PaddleTop.X = screenBounds.Center.X - (PaddleTop.Width / 2);
            PaddleTop.Y = 0;

            Ball = new Ball(GameConstants.DefaultBallSize, GameConstants.DefaultInitialBallSpeed, GameConstants.DefaultBallBumpSpeedIncreaseFactor)
            {
                X = screenBounds.Center.X - (GameConstants.DefaultBallSize / 2),
                Y = screenBounds.Center.Y - (GameConstants.DefaultBallSize / 2)

            };

            
            Background = new Background(screenBounds.Width, screenBounds.Height);

            SpritesForDrawList.Add(Background);
            SpritesForDrawList.Add(PaddleBottom);
            SpritesForDrawList.Add(PaddleTop);
            SpritesForDrawList.Add(Ball);

            Walls = new List<Wall>()
            {
                new Wall(-GameConstants.WallDefaultSize, 0, GameConstants.WallDefaultSize, screenBounds.Height),
                new Wall(screenBounds.Right, 0, GameConstants.WallDefaultSize, screenBounds.Height),
            };

            Goals = new List<Wall>()
            {
                new Wall(0, screenBounds.Height, screenBounds.Width, GameConstants.WallDefaultSize),
                new Wall(screenBounds.Top, -GameConstants.WallDefaultSize, screenBounds.Width, GameConstants.WallDefaultSize)
            };
            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Set textures
            Texture2D paddleTexture = Content.Load<Texture2D>("paddle");
            PaddleBottom.Texture = paddleTexture;
            PaddleTop.Texture = paddleTexture;
            Ball.Texture = Content.Load<Texture2D>("ball");
            Background.Texture = Content.Load<Texture2D>("background");

            
            // Load sounds
            // Start background music
            HitSound = Content.Load<SoundEffect>("hit");
            Music = Content.Load<Song>("music");
            //Music = Content.Load<Song>("uefa_theme");
            
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Music);

            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            var touchState = Keyboard.GetState();
            if (touchState.IsKeyDown(Keys.Left))
            {
                movePaddleLeft(PaddleBottom, gameTime);
            }
            if (touchState.IsKeyDown(Keys.Right))
            {
                movePaddleRight(PaddleBottom, gameTime);
            }

            if (touchState.IsKeyDown(Keys.A))
            {
                movePaddleLeft(PaddleTop, gameTime);
            }
            if (touchState.IsKeyDown(Keys.D))
            {
                movePaddleRight(PaddleTop, gameTime);
            }

            var screenBounds = GraphicsDevice.Viewport.Bounds;
            // D∈[-1 , 1]
            var ballPositionChange = Ball.Direction * MathHelper.Clamp((float)(gameTime.ElapsedGameTime.TotalMilliseconds * Ball.Speed), -5 , 5);
            Ball.X += ballPositionChange.X;
            Ball.Y += ballPositionChange.Y;

            // Ball - side walls
            if (Walls.Any(w => CollisionDetector.Overlaps(Ball, w)))
            {
                Vector2 vector;
                vector = Ball.Direction;
                vector.X = -vector.X;
                Ball.Direction = vector;
                Ball.Direction = vector;
                if (Ball.Speed * Ball.BumpSpeedIncreaseFactor <= GameConstants.MaxBallSpeed)
                {
                    Ball.Speed = Ball.Speed * Ball.BumpSpeedIncreaseFactor;
                }
                
            }
            // Ball - winning walls
            if (Goals.Any(w => CollisionDetector.Overlaps(Ball, w)))
            {
                Ball.X = screenBounds.Center.ToVector2().X;
                Ball.Y = screenBounds.Center.ToVector2().Y;
                Ball.Speed = GameConstants.DefaultInitialBallSpeed;
                HitSound.Play();
            }
            // Paddle - ball collision
            if (CollisionDetector.Overlaps(Ball, PaddleTop) && Ball.Direction.Y < 0 || (CollisionDetector.Overlaps(Ball, PaddleBottom) && Ball.Direction.Y > 0))
            {
                Vector2 vector;
                vector = Ball.Direction;
                vector.Y = -vector.Y;

                Ball.Direction = vector;
                if (Ball.Speed * Ball.BumpSpeedIncreaseFactor <= GameConstants.MaxBallSpeed)
                {
                    Ball.Speed = Ball.Speed * Ball.BumpSpeedIncreaseFactor;
                }
            }

            base.Update(gameTime);
        }

        private void movePaddleRight(Paddle paddle, GameTime gameTime)
        {
            var screenBounds = GraphicsDevice.Viewport.Bounds;
            paddle.X = MathHelper.Clamp(paddle.X + (float)(paddle.Speed * gameTime.ElapsedGameTime.TotalMilliseconds), 0, screenBounds.Width - paddle.Width) ;
            
        }

        private void movePaddleLeft(Paddle paddle, GameTime gameTime)
        {
            var screenBounds = GraphicsDevice.Viewport.Bounds;
            paddle.X = MathHelper.Clamp(paddle.X - (float)(paddle.Speed * gameTime.ElapsedGameTime.TotalMilliseconds), 0, screenBounds.Width - paddle.Width);
        }


        

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            spriteBatch.Begin();
            for (int i = 0; i < SpritesForDrawList.Count; i++)
            {
                SpritesForDrawList.GetElement(i).DrawSpriteOnScreen(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);

        }
    }
}
