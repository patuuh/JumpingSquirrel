using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Controls;
using Jypeli.Assets;
using Jypeli.Widgets;

/// @author Jokela Patrik, Viljamaa Juuso
/// @version 1.0

public class JumpingSquirrel : PhysicsGame
{
    private int kenttaNro = 1;
    private int vaikeus = 1;
    private const double nopeus = 200;
    private const double hyppyNopeus = 1000;
    private const int RUUDUN_KOKO = 40;
    private Random rand = new Random();
    private Vector LiikutaKameraa = new Vector(0, 30);
    private Vector stop = new Vector(0, 0);
    

    private PhysicsObject taso;
    private PhysicsObject killbox;
    private PlatformCharacter pelaaja1;

    private Image[] pelaajanHyppy = LoadImages("hyppykurre");
    private Image[] pelaajanIdle = LoadImages("kurre2");

    private Image oksaKuva = LoadImage("oksa");
    private Image runkoKuva = LoadImage("runko4");
    private Image acornKuva = LoadImage("acorn");
    private Image taustaKuva = LoadImage("tausta");
    private Image tausta2Kuva = LoadImage("tausta2");
    private Image tausta3Kuva = LoadImage("tausta3");
    private Image tausta4Kuva = LoadImage("tausta4");
    private Image lintuKuva = LoadImage("ankka");


    private Shape lintuMuoto;

    private SoundEffect hyppyAani = LoadSoundEffect("pomppu");

    /// <summary>
    /// Alustetaan kentälle painovoima
    /// Kutsutaan aliohjelmia luomaan kenttä, alkuvalikko ja näppäimet
    /// Asetetaan kamera
    /// </summary>
    public override void Begin()
    {
        ClearAll();
        Gravity = new Vector(0, -1000);
        if (kenttaNro == 1) LuoKentta("kentta1");
        else if (kenttaNro == 2) LuoKentta("kentta2");
        if (kenttaNro >= 3) Exit();

        Camera.Y = Level.Bottom;
        Camera.ZoomFactor = 1.2;
        Camera.StayInLevel = true;
        pelaaja1.CollisionIgnoreGroup = 2;
        LisaaNappaimet();
        //topLista = DataStorage.TryLoad<ScoreList>(topLista, "pisteet.xml");
        Alkuvalikko();
    }


    /// <summary>
    /// Aliohjelma tyhjentää kaiken, jonka jälkeen valitsee kentän riippuen mitä kenttää ollaan 
    /// pelaamassa
    /// Asettaa kameran
    /// Kutsuu aliohjelmat lisätäkseen näppäimet ja alkuvalikon
    /// </summary>
    public void SeuraavaKentta()
    {
        ClearAll();
        //aikaLaskuri.Reset();

        Gravity = new Vector(0, -1000);
        if (kenttaNro == 1) LuoKentta("kentta1");
        else if (kenttaNro == 2) LuoKentta("kentta2");
        if (kenttaNro >= 3) Exit();

        Camera.Y = Level.Bottom;
        Camera.ZoomFactor = 1.2;
        Camera.StayInLevel = true;
        LisaaNappaimet();
        AloitaPeli();
    }


    /// <summary>
    /// Luodaan Kenttä tilemapin perusteella
    /// Lisätään vihu lintuja
    /// Lisätään kenttään tausta ja reunat
    /// </summary>
    public void LuoKentta(string kenttaTiedostonNimi)
    {
        TileMap kentta = TileMap.FromLevelAsset(kenttaTiedostonNimi);
        kentta.SetTileMethod('#', LisaaRunko);
        kentta.SetTileMethod('*', LisaaOksa);
        kentta.SetTileMethod('N', LisaaPelaaja);
        kentta.SetTileMethod('@', LisaaTaso);
        kentta.SetTileMethod('%', LisaaAcorn);
        kentta.Execute(RUUDUN_KOKO, RUUDUN_KOKO);

        //LuoAikaLaskuri();
        ///Tässä käytetty Demo 6 oppeja, silmukka
        if (kenttaNro == 1)
        {
            GameObject tausta = new GameObject(900, Screen.Height, 0, -60);
            Add(tausta, -3);
            LisaaKillBox(0, -630);
            if (vaikeus == 0)
            {
                Level.Background.Color = new Color(146, 235, 255);
                tausta.Image = taustaKuva;
                for (int i = 0; i < 5; i++)
                {
                    LisaaLintu(this, rand.Next(-150, 200), rand.Next(-200, 400)); // Kentän vasen laita -150, oikea laita 200
                }
            }
            if (vaikeus == 1)
            {
                Level.Background.Color = new Color(65, 80, 251);
                tausta.Image = tausta2Kuva;
                for (int i = 0; i < 8; i++)
                {
                    LisaaLintu(this, rand.Next(-150, 200), rand.Next(-200, 400)); // Kentän vasen laita -150, oikea laita 200
                }
            }
            if (vaikeus == 2)
            {
                Level.Background.Color = new Color(0, 11, 134);
                tausta.Image = tausta3Kuva;
                for (int i = 0; i < 11; i++)
                {
                    LisaaLintu(this, rand.Next(-150, 200), rand.Next(-200, 400)); // Kentän vasen laita -150, oikea laita 200
                }
            }
            if (vaikeus == 3)
            {
                Level.Background.Color = new Color(0, 0, 0);
                tausta.Image = tausta4Kuva;
                for (int i = 0; i < 15; i++)
                {
                    LisaaLintu(this, rand.Next(-150, 200), rand.Next(-200, 400)); // Kentän vasen laita -150, oikea laita 200
                }
            }
        }
        if (kenttaNro == 2)
        {
            GameObject tausta = new GameObject(900, 1100, 0, -150);
            Add(tausta, -3);
            LisaaKillBox(0, -1150);
            if (vaikeus == 0)
            {
                Level.Background.Color = new Color(146, 235, 255);
                tausta.Image = taustaKuva;
                for (int i = 0; i < 5; i++)
                {
                    LisaaLintu(this, rand.Next(-150, 200), rand.Next(-200, 800)); // Kentän vasen laita -150, oikea laita 200
                }
            }
            if (vaikeus == 1)
            {
                Level.Background.Color = new Color(65, 80, 251);
                tausta.Image = tausta2Kuva;
                for (int i = 0; i < 10; i++)
                {
                    LisaaLintu(this, rand.Next(-150, 200), rand.Next(-200, 800)); // Kentän vasen laita -150, oikea laita 200
                }
            }
            if (vaikeus == 2)
            {
                Level.Background.Color = new Color(0, 11, 134);
                tausta.Image = tausta3Kuva;
                for (int i = 0; i < 14; i++)
                {
                    LisaaLintu(this, rand.Next(-150, 200), rand.Next(-200, 800)); // Kentän vasen laita -150, oikea laita 200 
                }
            }
            if (vaikeus == 3)
            {
                Level.Background.Color = new Color(0, 0, 0);
                tausta.Image = tausta4Kuva;
                for (int i = 0; i < 19; i++)
                {
                    LisaaLintu(this, rand.Next(-150, 200), rand.Next(-200, 800)); // Kentän vasen laita -150, oikea laita 200 
                }
            }
        }
        //Level.Background.CreateGradient(Color.White, Color.SkyBlue);
        Layers[-3].RelativeTransition = new Vector(0.5, 0.5);
    }


    private void Easy()
    {
        vaikeus = 0;
        Begin();
    }


    private void Medium()
    {
        vaikeus = 1;
        Begin();
    }


    private void Hard()
    {
        vaikeus = 2;
        Begin();
    }


    private void Intense()
    {
        vaikeus = 3;
        Begin();
    }


    public void VaikeustasoValikko()
    {
        MultiSelectWindow vaikeustasovalikko = new MultiSelectWindow("Choose Your Difficulty Level", "Easy", "Regular", "Hard", "Intense");
        vaikeustasovalikko.DefaultCancel = -1;
        vaikeustasovalikko.AddItemHandler(0, Easy);
        vaikeustasovalikko.AddItemHandler(1, Medium);
        vaikeustasovalikko.AddItemHandler(2, Hard);
        vaikeustasovalikko.AddItemHandler(3, Intense);
        Add(vaikeustasovalikko);
    }


    public void VoittoValikko()
    {
        MultiSelectWindow vaikeustasovalikko = new MultiSelectWindow("You won the Game!", "Main Menu", "Exit");
        vaikeustasovalikko.DefaultCancel = -1;
        vaikeustasovalikko.AddItemHandler(0, Begin);
        vaikeustasovalikko.AddItemHandler(1, Exit);
        Add(vaikeustasovalikko);
    }


    /// <summary>
    /// Aliohjelma luo alkuvalikon
    /// </summary>
    public void Alkuvalikko()
    {
        //MultiSelectWindow alkuValikko = new MultiSelectWindow("Pelin alkuvalikko", "Aloita peli", "Parhaat pisteet", "Lopeta");
        MultiSelectWindow alkuValikko = new MultiSelectWindow("Main Menu", "Start Game", "Difficulty", "Exit");
        alkuValikko.DefaultCancel = 3;
        alkuValikko.AddItemHandler(0, AloitaPeli);
        alkuValikko.AddItemHandler(1, VaikeustasoValikko);
        alkuValikko.AddItemHandler(2, Exit);
        Add(alkuValikko);
    }

    
    /// <summary>
    /// Aliohjelma luo valikon kuoleman jälkeen
    /// </summary>
    public void Kuolemavalikko()
    {
        //MultiSelectWindow alkuValikko = new MultiSelectWindow("Pelin alkuvalikko", "Aloita peli", "Parhaat pisteet", "Lopeta");
        Camera.Velocity = stop;
        MultiSelectWindow kuolemavalikko = new MultiSelectWindow("You Died!", "Restart", "Main Menu", "Exit");
        kuolemavalikko.DefaultCancel = 3;
        kuolemavalikko.AddItemHandler(0, SeuraavaKentta);
        kuolemavalikko.AddItemHandler(1, Begin);
        kuolemavalikko.AddItemHandler(2, Exit);
        Add(kuolemavalikko);
    }


    /// <summary>
    /// Aliohjelma aloittaa pelin ja aloittaa kameran liikkumisen
    /// </summary>
    public void AloitaPeli()
    {
        Timer.SingleShot(1.5, KameranLiike);
        pelaaja1.CollisionIgnoreGroup = 0;
        //aikaLaskuri.Start();
    }


    /// <summary>
    /// Liikuttaa kameraa ja taustaa
    /// </summary>
    private void KameranLiike()
    {
        Camera.Velocity = LiikutaKameraa;
        killbox.Velocity = LiikutaKameraa;
    }
    

    /// <summary>
    /// Pelaajan kuolema tapahtuma. tuhoaa pelaajan ja kutsuu SeuraavaKentta aliohjelman
    /// </summary>
    public void PelaajaKuoli() // liittyy pisteiden laskuun
    {
        pelaaja1.Destroy();
        //aikaLaskuri.Stop();
        Timer.SingleShot(1, Kuolemavalikko);
    }


    /// <summary>
    /// Lisää peliin näppäimet
    /// </summary>
    public void LisaaNappaimet()
    {
        Keyboard.Listen(Key.F1, ButtonState.Pressed, ShowControlHelp, "Näytä ohjeet");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");

        Keyboard.Listen(Key.Left, ButtonState.Down, Liikuta, "Liikkuu vasemmalle", pelaaja1, -nopeus);
        Keyboard.Listen(Key.Right, ButtonState.Down, Liikuta, "Liikkuu oikealle", pelaaja1, nopeus);
        Keyboard.Listen(Key.Up, ButtonState.Pressed, Hyppaa, "Pelaaja hyppää", pelaaja1, hyppyNopeus);

        ControllerOne.Listen(Button.Back, ButtonState.Pressed, Exit, "Poistu pelistä");

        ControllerOne.Listen(Button.DPadLeft, ButtonState.Down, Liikuta, "Pelaaja liikkuu vasemmalle", pelaaja1, -nopeus);
        ControllerOne.Listen(Button.DPadRight, ButtonState.Down, Liikuta, "Pelaaja liikkuu oikealle", pelaaja1, nopeus);
        ControllerOne.Listen(Button.A, ButtonState.Pressed, Hyppaa, "Pelaaja hyppää", pelaaja1, hyppyNopeus);

        Keyboard.Listen(Key.R, ButtonState.Pressed, PelaajaKuoli, "Aloita kenttä alusta");

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");

    }


    


    /// <summary>
    /// Aliohjelma luo "vihu" olion, lintu
    /// </summary>
    /// <param name="peli">nykyinen peli</param>
    /// <param name="x">linnun x-koordinaatti</param>
    /// <param name="y">linnun y-koordinaatti</param>
    private void LisaaLintu(PhysicsGame peli, double x, double y)
    {
        PhysicsObject lintu = new PhysicsObject(20, 20, Shape.Rectangle);
        lintu.Position = new Vector(x, y);
        lintu.Image = lintuKuva;
        lintu.X = x;
        lintu.Y = y;
        lintu.IgnoresGravity = true;
        lintuMuoto = Shape.FromImage(lintuKuva);
        lintu.Tag = "lintu";
        RandomMoverBrain satunnaisAivot = new RandomMoverBrain(200);
        satunnaisAivot.ChangeMovementSeconds = 3;
        lintu.Brain = satunnaisAivot;
        lintu.CollisionIgnoreGroup = 2;
        Add(lintu);
    }


    /// <summary>
    /// Aliohjelma luo puun rungon jotka toimii kentän seininä
    /// </summary>
    /// <param name="paikka">Paikka johon runko syntyy</param>
    /// <param name="leveys">rungon leveys</param>
    /// <param name="korkeus">rungon korkeus</param>
    private void LisaaRunko(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject runko = PhysicsObject.CreateStaticObject(leveys, korkeus);
        runko.Position = paikka;
        runko.Image = runkoKuva;
        runko.CollisionIgnoreGroup = 1;
        Add(runko);
    }


    /// <summary>
    /// Luo tason joka toimii kentän alareunana
    /// </summary>
    /// <param name="paikka">Sijainti</param>
    /// <param name="leveys">Leveys</param>
    /// <param name="korkeus">Korkeus</param>
    private void LisaaTaso(Vector paikka, double leveys, double korkeus)
    {
        taso = PhysicsObject.CreateStaticObject(leveys, korkeus);
        taso.Position = paikka;
        taso.Color = Color.Green;
        taso.CollisionIgnoreGroup = 1;
        Add(taso);
    }


    /// <summary>
    /// Luo tason joka toimii kentän alareunana
    /// </summary>
    /// <param name="paikka">Sijainti</param>
    /// <param name="leveys">Leveys</param>
    /// <param name="korkeus">Korkeus</param>
    private void LisaaKillBox(int x, int y, double leveys = 450, double korkeus = 5)
    {
        killbox = new PhysicsObject(370, 5, Shape.Rectangle);
        killbox.X = x;
        killbox.Y = y;
        killbox.IgnoresGravity = true;
        killbox.IgnoresCollisionResponse = true;
        killbox.Color = Color.Transparent;
        killbox.Tag = "killbox";
        Add(killbox);
    }

    /// <summary>
    /// Luo Acorn jota pelaaja yrittää tavoitella. Saatuaan tämän, kenttä on läpäisty
    /// </summary>
    /// <param name="paikka">Paikka johon Acorn syntyy</param>
    /// <param name="leveys">Acorn leveys</param>
    /// <param name="korkeus">Acorn korkeus</param>
    private void LisaaAcorn(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject acorn = PhysicsObject.CreateStaticObject(leveys, korkeus);
        acorn.IgnoresCollisionResponse = true;
        acorn.Position = paikka;
        acorn.Image = acornKuva;
        acorn.Tag = "acorn";
        acorn.CollisionIgnoreGroup = 1;
        Add(acorn);
    }


    /// <summary>
    /// Luo oksan jonka päälle pelaajan pitää hyppiä
    /// </summary>
    /// <param name="paikka">Oksan sijainti</param>
    /// <param name="leveys">Oksan leveys</param>
    /// <param name="korkeus">Oksan korkeus</param>
    private void LisaaOksa(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject oksa = PhysicsObject.CreateStaticObject(leveys, korkeus);
        oksa.Position = paikka;
        oksa.Image = oksaKuva;
        oksa.CollisionIgnoreGroup = 1;
        oksa.Tag = "oksa";
        Add(oksa);
    }
    

    /// <summary>
    /// Luo pelaajan
    /// </summary>
    /// <param name="paikka">pelaajan aloitus sijainti</param>
    /// <param name="leveys">pelaajan leveys</param>
    /// <param name="korkeus">pelaajan korkus</param>
    public void LisaaPelaaja(Vector paikka, double leveys, double korkeus)
    {
        pelaaja1 = new PlatformCharacter(leveys, korkeus);
        pelaaja1.Position = paikka;
        pelaaja1.Mass = 4.0;
        pelaaja1.Tag = "pelaaja";
        AddCollisionHandler(pelaaja1, "acorn", TormaaAcorn);
        AddCollisionHandler(pelaaja1, "lintu", TormaaLintuun);
        AddCollisionHandler(pelaaja1, "oksa", TormaaOksaan);
        AddCollisionHandler(pelaaja1, "killbox", TormaaKillBox);
        pelaaja1.AnimJump = new Animation(pelaajanHyppy);
        pelaaja1.AnimWalk = new Animation(pelaajanIdle);
        pelaaja1.AnimIdle = new Animation(pelaajanIdle);
        Add(pelaaja1);
    }


    /// <summary>
    /// Hahmon liikkumis nopeus
    /// </summary>
    /// <param name="hahmo">Pelaaja</param>
    /// <param name="nopeus">Nopeus millä pelaaja liikkuu</param>
    private void Liikuta(PlatformCharacter hahmo, double nopeus)
    {
        hahmo.Walk(nopeus);
    }


    /// <summary>
    /// Hahmon hyppynopeus
    /// </summary>
    /// <param name="hahmo">pelaaja</param>
    /// <param name="nopeus">hahmon nopeus millä hän hyppää</param>
    private void Hyppaa(PlatformCharacter hahmo, double nopeus)
    {
        hahmo.Jump(nopeus);
        hyppyAani.Play();
    }

    private void TormaaKillBox(PhysicsObject hahmo, PhysicsObject killbox)
    {
        MessageDisplay.Add("Tipuit, Kuolit! Aloita alusta");
        pelaaja1.Destroy();
        Timer.SingleShot(1, Kuolemavalikko);
    }




    private void TormaaOksaan(PhysicsObject hahmo, PhysicsObject oksa)
    {
        //tähän miten oksa hajoaa kun pelaaja on tarpeeksi kauan yhdellä oksalla
    }


    /// <summary>
    /// Tapahtuma kun pelaaja törmää Acorn
    /// viesti kerrotaan että taso läpäisty
    /// Käynnistää seuraavan kentän
    /// </summary>
    /// <param name="hahmo">pelaaja</param>
    /// <param name="acorn">Acorn mihin osutaan</param>
    private void TormaaAcorn(PhysicsObject hahmo, PhysicsObject acorn)
    {
        acorn.Destroy();
        if (kenttaNro == 2)
        {
            VoittoValikko();
        }
        else
        {
            MessageDisplay.Add("Keräsit Tammenterhon! Läpäisit tason");
            kenttaNro++; //näillä pääsee seuraavaan kenttään
            Timer.SingleShot(1, Begin);
        }
    }


    /// <summary>
    /// Tapahtuma kun pelaaja törmää lintuun
    /// Luodaan räjähdys
    /// Luodaan viesti "pelaaja kuollut"
    /// Kutsutaan aliohjelmaa PelaajaKuoli viiveellä jotta räjähdys ehtii näkymään
    /// </summary>
    /// <param name="hahmo"></param>
    /// <param name="lintu"></param>
    private void TormaaLintuun(PhysicsObject hahmo, PhysicsObject lintu)
    {
        //aikaLaskuri.Stop();
        Explosion rajahdys = new Explosion(lintu.Width * 3);
        rajahdys.Image = LoadImage("rajahdys2");
        rajahdys.Position = lintu.Position;
        rajahdys.UseShockWave = true;
        this.Add(rajahdys);
        pelaaja1.Destroy();
        MessageDisplay.Add("Osuit lintuun, Kuolit! Aloita alusta");
        Timer.SingleShot(1, Kuolemavalikko);
    }
}
