using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System.Linq;

namespace AstroOdyssey
{
    public class LocalizationHelper : ILocalizationHelper
    {
        #region Fields

        private LocalizationKey[] LOCALIZATION_KEYS = new LocalizationKey[]
        {
            #region Game Start Page

		    new LocalizationKey(key: "ApplicationName_Header", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Astro Odyssey"),
                new ("bn", "অ্যাস্ট্রো ওডিসি"),
                new ("de", "Astro-Odyssee"),
                new ("fr", "Astro Odyssée"),
            }),
            new LocalizationKey(key: "GameStartPage_Tagline", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "A classic rock metal ⚡ space shooter on WebAssembly."),
                new ("bn", "WebAssembly-এ একটি ক্লাসিক রক মেটাল ⚡ স্পেস শ্যুটার।"),
                new ("de", "Ein klassischer Rock Metal ⚡ Weltraum-Shooter auf WebAssembly."),
                new ("fr", "Un jeu de tir spatial classique rock métal ⚡ sur WebAssembly."),
            }),

            new LocalizationKey(key: "GameStartPage_BanglaButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Bangla"),
                new ("bn", "বাংলা"),
                new ("de", "Bangla"),
                new ("fr", "Bangla"),
            }),
            new LocalizationKey(key: "GameStartPage_DeutschButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Deutsch"),
                new ("bn", "ডয়েচ"),
                new ("de", "Deutsch"),
                new ("fr", "Deutsch"),
            }),
            new LocalizationKey(key: "GameStartPage_EnglishButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "English"),
                new ("bn", "ইংরেজি"),
                new ("de", "Englisch"),
                new ("fr", "Anglais"),
            }),
            new LocalizationKey(key: "GameStartPage_FrenchButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "French"),
                new ("bn", "ফরাসি"),
                new ("de", "Französisch"),
                new ("fr", "Français"),
            }),

            new LocalizationKey(key: "GameStartPage_PlayButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Start Game"),
                new ("bn", "খেলা শুরু করুন"),
                new ("de", "Spiel Beginnen"),
                new ("fr", "Démarrer Jeu"),
            }),

            new LocalizationKey(key: "GameStartPage_LogoutButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Logout"),
                new ("bn", "প্রস্থান"),
                new ("de", "Ausloggen"),
                new ("fr", "Se Déconnecter"),
            }),

            new LocalizationKey(key: "GameStartPage_BrandProfileButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Made with ❤️ by Asadullah Rifat"),
                new ("bn", "আসাদুল্লাহ রিফাত দ্বারা ❤️ দিয়ে তৈরি"),
                new ("de", "Hergestellt mit ❤️ von Asadullah Rifat"),
                new ("fr", "Fait avec ❤️ par Asadullah Rifat"),
            }),

            new LocalizationKey(key: "GameStartPage_WelcomeBackText", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "👋 Welcome Back!"),
                new ("bn", "👋 স্বাগতম!"),
                new ("de", "👋 Willkommen Xurück!"),
                new ("fr", "👋 Content De Te Revoir!"),
            }),

            new LocalizationKey(key: "LOADING_GAME_ASSETS", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Loading game assets..."),
                new ("bn", "গেমের সম্পদ লোড হচ্ছে ..."),
                new ("de", "Ladespiel -Vermögenswerte..."),
                new ("fr", "Chargement des actifs de jeu..."),
            }),
	        #endregion

            #region Ship Selection Page            

		    new LocalizationKey(key: "Antimony", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Antimony"),
                new ("bn", "অ্যান্টিমনি"),
                new ("de", "Antimon"),
                new ("fr", "Antimoine"),
            }),
            new LocalizationKey(key: "Bismuth", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Bismuth"),
                new ("bn", "বিসমাথ"),
                new ("de", "Wismut"),
                new ("fr", "Bismuth"),
            }),
            new LocalizationKey(key: "Curium", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Curium"),
                new ("bn", "কিউরিয়াম"),
                new ("de", "Gericht"),
                new ("fr", "Rechercher"),
            }),

            new LocalizationKey(key: "ShipSelectionPage_Header", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Welcome to"),
                new ("bn", "স্বাগতম"),
                new ("de", "Willkommen zu"),
                new ("fr", "Bienvenue à"),
            }),

            new LocalizationKey(key: "ShipSelectionPage_Tagline", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Choose a spaceship"),
                new ("bn", "একটি স্পেসশিপ চয়ন করুন"),
                new ("de", "Wähle ein Raumschiff"),
                new ("fr", "Choisis un vaisseau"),
            }),

            new LocalizationKey(key: "ShipSelectionPage_ControlInstructions", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Each spaceship offers a different gaming experience."),
                new ("bn", "প্রতিটি স্পেসশিপ একটি আলাদা গেমিং অভিজ্ঞতা সরবরাহ করে।"),
                new ("de", "Jedes Raumschiff bietet ein anderes Spielerlebnis."),
                new ("fr", "Chaque vaisseau spatial offre une expérience de jeu différente."),
            }),

            new LocalizationKey(key: "ShipSelectionPage_ChooseButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Select"),
                new ("bn", "নির্বাচন করুন"),
                new ("de", "Auswählen"),
                new ("fr", "Sélectionner"),
            }),

	        #endregion

            #region Game Instructions Page

            new LocalizationKey(key: "GameInstructionsPage_Tagline", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "How To Play"),
                new ("bn", "কিভাবে খেলতে হবে"),
                new ("de", "Spielanleitung"),
                new ("fr", "Comment Jouer"),
            }),

            new LocalizationKey(key: "GameInstructionsPage_ControlsText", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Controls"),
                new ("bn", "নিয়ন্ত্রণ"),
                new ("de", "Steuerung"),
                new ("fr", "Contrôle"),
            }),
            new LocalizationKey(key: "GameInstructionsPage_ControlsText2", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Move to ⬅️ or ➡️ with the arrow keys on your ⌨️ or use the touch screen of your 📱."),
                new ("bn", "আপনার ⌨ এ তীর কীগুলি দিয়ে ⬅ বা ➡ এ যান বা আপনার 📱 এর টাচ স্ক্রিনটি ব্যবহার করুন।"),
                new ("de", "Bewege dich nach ⬅️ oder ➡️ mit den Pfeiltasten auf deiner ⌨️ oder benutze den Touch Screen deines 📱."),
                new ("fr", "Déplace-toi vers ⬅️ ou ➡️ avec les touches fléchées de ton ⌨️ ou utilise l'écran tactile de ton 📱."),
            }),

            new LocalizationKey(key: "GameInstructionsPage_EnemiesText", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Enemies & Meteors"),
                new ("bn", "শত্রু ও উল্কা"),
                new ("de", "Gegner & Meteoriten"),
                new ("fr", "Adversaires & Météorites"),
            }),
            new LocalizationKey(key: "GameInstructionsPage_EnemiesText2", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Shoot them and avoid collision."),
                new ("bn", "তাদের গুলি করুন এবং সংঘর্ষ এড়িয়ে চলুন।"),
                new ("de", "Schießen Sie sie und vermeiden Sie Kollision."),
                new ("fr", "Tirez-les et évitez la collision."),
            }),

            new LocalizationKey(key: "GameInstructionsPage_BossesText", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Bosses"),
                new ("bn", "বস"),
                new ("de", "Bosse"),
                new ("fr", "Patrons"),
            }),
            new LocalizationKey(key: "GameInstructionsPage_BossesText2", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Defeat them to get to the next level."),
                new ("bn", "পরবর্তী স্তরে পৌঁছানোর জন্য তাদের পরাজিত করুন।"),
                new ("de", "Besige sie um ins nächste Level zu kommen."),
                new ("fr", "Battez-les pour passer au niveau suivant."),
            }),

            new LocalizationKey(key: "GameInstructionsPage_HealthText", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Healths"),
                new ("bn", "স্বাস্থ্য"),
                new ("de", "Leben"),
                new ("fr", "Vies"),
            }),
            new LocalizationKey(key: "GameInstructionsPage_HealthText2", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Collect them to repair your spaceship."),
                new ("bn", "আপনার স্পেসশিপটি মেরামত করতে এগুলি সংগ্রহ করুন।"),
                new ("de", "Sammle sie um dein Raumschiff zu reparieren."),
                new ("fr", "Rassemble-les pour réparer ton vaisseau spatial."),
            }),

            new LocalizationKey(key: "GameInstructionsPage_PowerupText", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Power-ups"),
                new ("bn", "শক্তি বৃদ্ধি"),
                new ("de", "Einschalten"),
                new ("fr", "Mises sous tension"),
            }),
            new LocalizationKey(key: "GameInstructionsPage_PowerupText2", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Collect them to get more powerful weapons."),
                new ("bn", "আরও শক্তিশালী অস্ত্র পেতে তাদের সংগ্রহ করুন।"),
                new ("de", "Sammle sie um mächtigere Waffen zu erhalten."),
                new ("fr", "Collectez-les pour obtenir des armes plus puissantes."),
            }),

            new LocalizationKey(key: "GameInstructionsPage_CollectiblesText", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Collectibles"),
                new ("bn", "সংগ্রহযোগ্য"),
                new ("de", "Sammlerobjekte"),
                new ("fr", "Objets de collection"),
            }),
            new LocalizationKey(key: "GameInstructionsPage_CollectiblesText2", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Collect them to increase your firepower and activate 2x score."),
                new ("bn", "আপনার ফায়ারপাওয়ার বাড়াতে এবং 2x স্কোর সক্রিয় করতে এগুলি সংগ্রহ করুন।"),
                new ("de", "Sammle sie ein, um deine Feuerkraft zu erhöhen und 2-fache Punkte zu aktivieren."),
                new ("fr", "Récolte-les pour augmenter ta puissance de feu et activer des points 2x."),
            }),

            new LocalizationKey(key: "GameInstructionsPage_PlayButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "👍 Okay"),
                new ("bn", "👍 ঠিক আছে"),
                new ("de", "👍 Okay"),
                new ("fr", "👍 D'accord"),
            }),

	        #endregion

            #region Game Play Page

		    new LocalizationKey(key: "BEAM_CANNON", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "BEAM CANNON"),
                new ("bn", "রশ্মি কামান"),
                new ("de", "STRAHLKANONE"),
                new ("fr", "CANON À FAISCEAU"),
            }),
            new LocalizationKey(key: "BLAZE_BLITZ", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "BLAZE BLITZ"),
                new ("bn", "ব্লেজ ব্লিটজ"),
                new ("de", "BLAZE BLITZ"),
                new ("fr", "BLAZE BLITZ"),
            }),
            new LocalizationKey(key: "PLASMA_BOMB", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "PLASMA BOMB"),
                new ("bn", "প্লাজমা বোমা"),
                new ("de", "Plasmabombe"),
                new ("fr", "BOMBE À PLASMA"),
            }),

            new LocalizationKey(key: "BOSS", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "BOSS"),
                new ("bn", "বস"),
                new ("de", "BOSS"),
                new ("fr", "CHEF"),
            }),
            new LocalizationKey(key: "CLOAK_DOWN", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "CLOAK DOWN"),
                new ("bn", "ক্লোক ডাউন"),
                new ("de", "KLAPPEN SIE SICH"),
                new ("fr", "CLOCHEZ-VOUS"),
            }),
            new LocalizationKey(key: "CLOAK_UP", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "CLOAK UP"),
                new ("bn", "ক্লোক আপ"),
                new ("de", "KLEIDE DICH"),
                new ("fr", "COUVREZ-VOUS"),
            }),
            new LocalizationKey(key: "FIRING_RATE_DECREASED", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "FIRING RATE DECREASED"),
                new ("bn", "গুলিবর্ষণের হার কমেছে"),
                new ("de", "FEUERRATE VERRINGERT"),
                new ("fr", "LA CADENCE DE TIR A DIMINUÉ"),
            }),
            new LocalizationKey(key: "FIRING_RATE_INCREASED", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "FIRING RATE INCREASED"),
                new ("bn", "গুলিবর্ষণের হার বেড়েছে"),
                new ("de", "FEUERRATE ERHÖHT"),
                new ("fr", "CADENCE DE TIR AUGMENTÉE"),
            }),
            new LocalizationKey(key: "SHIELD_DOWN", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "SHIELD DOWN"),
                new ("bn", "শিল্ড ডাউন"),
                new ("de", "SCHILD NACH UNTEN"),
                new ("fr", "BOUCLIER BAS"),
            }),
            new LocalizationKey(key: "SHIELD_UP", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "SHIELD UP"),
                new ("bn", "শিল্ড আপ"),
                new ("de", "SCHILD AUF"),
                new ("fr", "BOUCLIER"),
            }),

            new LocalizationKey(key: "COMPLETE", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "COMPLETE"),
                new ("bn", "সম্পূর্ণ"),
                new ("de", "KOMPLETT"),
                new ("fr", "ACHEVÉ"),
            }),
            new LocalizationKey(key: "ENEMY_APPROACHES", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "ENEMY APPROACHES"),
                new ("bn", "শত্রু কাছে পৌঁছেছে"),
                new ("de", "Feindliche Annäherungen"),
                new ("fr", "APPROCHES ENNEMIES"),
            }),
            new LocalizationKey(key: "FANTASTIC_GAME", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Fantastic game"),
                new ("bn", "চমত্কার খেলা"),
                new ("de", "Fantastisches Spiel"),
                new ("fr", "Jeu fantastique"),
            }),
            new LocalizationKey(key: "GAME_PAUSED", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "GAME PAUSED"),
                new ("bn", "খেলা থামানো হয়েছে"),
                new ("de", "DAS SPIEL PAUSIERT"),
                new ("fr", "JEU EN PAUSE"),
            }),
            new LocalizationKey(key: "GOOD_GAME", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Good game"),
                new ("bn", "ভাল খেলা"),
                new ("de", "Gute Partie"),
                new ("fr", "Bon jeu"),
            }),
            new LocalizationKey(key: "GREAT_GAME", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Great game"),
                new ("bn", "অসাধারন খেলা"),
                new ("de", "Tolles Spiel"),
                new ("fr", "bon jeu"),
            }),
            new LocalizationKey(key: "LEVEL", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "LEVEL"),
                new ("bn", "স্তর"),
                new ("de", "EBEN"),
                new ("fr", "NIVEAU"),
            }),
            new LocalizationKey(key: "NO_LUCK", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "No luck"),
                new ("bn", "দুর্ভাগ্য"),
                new ("de", "Kein Glück"),
                new ("fr", "Pas de chance"),
            }),
            new LocalizationKey(key: "POWER_DOWN", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "POWER DOWN"),
                new ("bn", "ক্ষমতা হ্রাস"),
                new ("de", "STROMAUSFALL"),
                new ("fr", "ÉTEINDRE"),
            }),
            new LocalizationKey(key: "QUIT_GAME", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "QUIT GAME?"),
                new ("bn", "খেলা বন্ধ?"),
                new ("de", "SPIEL VERLASSEN?"),
                new ("fr", "QUITTER LE JEU?"),
            }),
            new LocalizationKey(key: "SCORE", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Score"),
                new ("bn", "স্কোর"),
                new ("de", "Punktzahl"),
                new ("fr", "Score"),
            }),
            new LocalizationKey(key: "YOUR_SCORE", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Your Score"),
                new ("bn", "আপনার ফলাফল"),
                new ("de", "Ihr Ergebnis"),
                new ("fr", "Ton score"),
            }),
            new LocalizationKey(key: "PERSONAL_BEST_SCORE", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "🤘 Personal Best Score"),
                new ("bn", "🤘 ব্যক্তিগত সেরা স্কোর"),
                new ("de", "🤘 Persönliche Beste Punktzahl"),
                new ("fr", "🤘 Meilleur Score personnel"),
            }),
            new LocalizationKey(key: "LAST_GAME_SCORE", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "🕹️ Last Game Score"),
                new ("bn", "🕹️ শেষ গেম স্কোর"),
                new ("de", "🕹️ Letzter Spielpunkt"),
                new ("fr", "🕹️ Score du dernier match"),
            }),

            new LocalizationKey(key: "DESTROYED", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Destroyed"),
                new ("bn", "ধ্বংস"),
                new ("de", "Zerstört"),
                new ("fr", "Détruits"),
            }),
            new LocalizationKey(key: "ENEMIES_DESTROYED", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Enemies"),
                new ("bn", "শত্রু"),
                new ("de", "Feinde"),
                new ("fr", "Ennemis"),
            }),
            new LocalizationKey(key: "METEORS_DESTROYED", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Meteors"),
                new ("bn", "উল্কা"),
                new ("de", "Meteoren"),
                new ("fr", "Météores"),
            }),
            new LocalizationKey(key: "BOSSES_DESTROYED", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Bosses"),
                new ("bn", "বস"),
                new ("de", "Bosse"),
                new ("fr", "Boss"),
            }),
            new LocalizationKey(key: "COLLECTIBLES_COLLECTED", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Collectibles"),
                new ("bn", "সংগ্রহযোগ্য"),
                new ("de", "Sammlerstücke"),
                new ("fr", "Objets de collection"),
            }),

            new LocalizationKey(key: "SHIP_REPAIRED", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "SHIP REPAIRED"),
                new ("bn", "জাহাজ মেরামত"),
                new ("de", "SCHIFF REPARIERT"),
                new ("fr", "NAVIRE RÉPARÉ"),
            }),

            new LocalizationKey(key: "SONIC_BLAST", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "SONIC BLAST"),
                new ("bn", "সোনিক বিস্ফোরণ"),
                new ("de", "SONIC BLAST"),
                new ("fr", "SONIC SONIC"),
            }),
            new LocalizationKey(key: "SUPREME_GAME", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Supreme game"),
                new ("bn", "সর্বোচ্চ খেলা"),
                new ("de", "Höchstes Spiel"),
                new ("fr", "Jeu suprême"),
            }),
            new LocalizationKey(key: "TAP_ON_SCREEN_TO_BEGIN", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "TAP ON THE SCREEN TO BEGIN"),
                new ("bn", "শুরু করতে স্ক্রিনে ট্যাপ করুন"),
                new ("de", "TIPPEN SIE AUF DEN BILDSCHIRM, UM ZU BEGINNEN"),
                new ("fr", "APPUYEZ SUR L'ÉCRAN POUR COMMENCER"),
            }),
            new LocalizationKey(key: "TAP_TO_QUIT", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "TAP TO QUIT"),
                new ("bn", "প্রস্থান করতে ট্যাপ করুন"),
                new ("de", "TIPPEN SIE ZUM BEENDEN"),
                new ("fr", "APPUYER POUR QUITTER"),
            }),
            new LocalizationKey(key: "TAP_TO_RESUME", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "TAP TO RESUME"),
                new ("bn", "আবার শুরু করতে ট্যাপ করুন"),
                new ("de", "TIPPEN, UM WEITERZUFAHREN"),
                new ("fr", "APPUYER POUR REPRENDRE"),
            }),

            new LocalizationKey(key: "SCORE_MULTIPLIER_ON", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "2x SCORE ON"),
                new ("bn", "2x স্কোর চালু"),
                new ("de", "2x Punktzahl auf"),
                new ("fr", "2x score sur"),
            }),
            new LocalizationKey(key: "SCORE_MULTIPLIER_OFF", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "COLLECT MORE GUITARS"),
                new ("bn", "আরও গিটার সংগ্রহ করুন"),
                new ("de", "SAMMLE MEHR GITARREN"),
                new ("fr", "RECUEILLIR PLUS DE GUITARES"),
            }),

	        #endregion

            #region Game Over Page

            new LocalizationKey(key: "GameOverPage_SignupPromptText", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "You could earn your place on the leaderboard if you login."),
                new ("bn", "লগইন করলে আপনি লিডারবোর্ডে আপনার জায়গা অর্জন করতে পারেন।"),
                new ("de", "Sie können sich Ihren Platz in der Rangliste verdienen, wenn Sie sich anmelden."),
                new ("fr", "Vous pouvez gagner votre place dans le classement si vous vous connectez."),
            }),

            new LocalizationKey(key: "GameOverPage_Tagline", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Game Over"),
                new ("bn", "খেলা শেষ"),
                new ("de", "Spiel Ist Aus"),
                new ("fr", "Jeu Terminé"),
            }),

            new LocalizationKey(key: "GameOverPage_PlayAgainButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Play Again"),
                new ("bn", "আবার খেলুন"),
                new ("de", "Nochmal Abspielen"),
                new ("fr", "Rejouer"),
            }),

            new LocalizationKey(key: "GameOverPage_LeaderboardButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Leaderboard"),
                new ("bn", "লিডারবোর্ড"),
                new ("de", "Bestenliste"),
                new ("fr", "Classement"),
            }),

	        #endregion

            #region Game Login Page

            new LocalizationKey(key: "GameLoginPage_UserNameBox", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Username or Email"),
                new ("bn", "ব্যবহারকারী নাম বা ইমেল"),
                new ("de", "Benutzername oder E-Mail-Adresse"),
                new ("fr", "Nom d'utilisateur ou email"),
            }),

            new LocalizationKey(key: "GameLoginPage_PasswordBox", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Password"),
                new ("bn", "পাসওয়ার্ড"),
                new ("de", "Passwort"),
                new ("fr", "Mot de passe"),
            }),

            new LocalizationKey(key: "GameLoginPage_LoginButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Login"),
                new ("bn", "প্রবেশ করুন"),
                new ("de", "Anmeldung"),
                new ("fr", "Connexion"),
            }),

            new LocalizationKey(key: "GameLoginPage_RegisterButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "No account yet?"),
                new ("bn", "এখনও কোন অ্যাকাউন্ট নেই?"),
                new ("de", "Noch keinen Account?"),
                new ("fr", "Pas encore de compte?"),
            }),

        	#endregion

            #region Game Signup Page

            new LocalizationKey(key: "GameSignupPage_UserFullNameBox", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Enter full name"),
                new ("bn", "পুরো নাম লিখুন"),
                new ("de", "Geben Sie den vollständigen Namen ein"),
                new ("fr", "Entrez le nom complet"),
            }),

            new LocalizationKey(key: "GameSignupPage_UserEmailBox", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Enter email"),
                new ("bn", "ইমেইল প্রদান করুন"),
                new ("de", "Email eingeben"),
                new ("fr", "Entrez l'e-mail"),
            }),

            new LocalizationKey(key: "GameSignupPage_UserNameBox", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Set a username"),
                new ("bn", "একটি ব্যবহারকারীর নাম সেট করুন"),
                new ("de", "Auf Benutzername setzen"),
                new ("fr", "Définir sur nom d'utilisateur"),
            }),

            new LocalizationKey(key: "GameSignupPage_PasswordBox", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Set a password"),
                new ("bn", "একটি পাসওয়ার্ড সেট করুন"),
                new ("de", "Legen Sie ein Passwort fest"),
                new ("fr", "Définir un mot de passe"),
            }),

            new LocalizationKey(key: "GameSignupPage_ConfirmPasswordBox", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Confirm password"),
                new ("bn", "পাসওয়ার্ড নিশ্চিত করুন"),
                new ("de", "Passwort bestätigen"),
                new ("fr", "Confirmez le mot de passe"),
            }),

            new LocalizationKey(key: "GameSignupPage_SignupButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Signup"),
                new ("bn", "নিবন্ধন করুন"),
                new ("de", "Anmelden"),
                new ("fr", "Se connecter"),
            }),

            new LocalizationKey(key: "GameSignupPage_LoginButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "You already have an account?"),
                new ("bn", "আপনার ইতিমধ্যেই একটি অ্যাকাউন্ট আছে?"),
                new ("de", "Du hast bereits einen Account?"),
                new ("fr", "Tu as déjà un compte ?"),
            }),

            new LocalizationKey(key: "GameSignupPage_ConfirmCheckBox", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Yes, I agree to the conditions of participation."),
                new ("bn", "হ্যাঁ, আমি অংশগ্রহণের শর্তে সম্মত।"),
                new ("de", "Ja, ich stimme den Teilnahmebedingungen zu."),
                new ("fr", "Oui, j'accepte les conditions de participation."),
            }),

            new LocalizationKey(key: "PASSWORDS_DIDNT_MATCH", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Passwords didn't match"),
                new ("bn", "পাসওয়ার্ড মেলে না"),
                new ("de", "Passwörter stimmten nicht überein"),
                new ("fr", "Les mots de passe ne correspondent pas"),
            }),

            new LocalizationKey(key: "PASSWORDS_MATCHED", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Passwords matched"),
                new ("bn", "পাসওয়ার্ড মিলেছে"),
                new ("de", "Passwörter übereinstimmen"),
                new ("fr", "Mots de passe correspondants"),
            }),

            new LocalizationKey(key: "LENGTH_MUST_BE_GREATER_THAN_8_CHARS", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Password must be at least eight characters"),
                new ("bn", "পাসওয়ার্ড অবশ্যই কমপক্ষে আটটি অক্ষর হতে হবে"),
                new ("de", "Das Passwort muss mindestens acht Zeichen betragen"),
                new ("fr", "Le mot de passe doit comporter au moins huit caractères"),
            }),

            new LocalizationKey(key: "LENGTH_MUST_BE_LESS_THAN_14_CHARS", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Password must be less than fourteen characters"),
                new ("bn", "পাসওয়ার্ড অবশ্যই চৌদ্দ অক্ষরের চেয়ে কম হতে হবে"),
                new ("de", "Das Passwort muss weniger als vierzehn Zeichen betragen"),
                new ("fr", "Le mot de passe doit être inférieur à quatorze caractères"),
            }),

            new LocalizationKey(key: "MUST_CONTAIN_ONE_UPPERCASE_CHAR", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Password must contain one uppercase character"),
                new ("bn", "পাসওয়ার্ডে অবশ্যই একটি বড় হাতের অক্ষর থাকতে হবে"),
                new ("de", "Passwort muss einen Großbuchstaben enthalten"),
                new ("fr", "Le mot de passe doit contenir un caractère majuscule"),
            }),

            new LocalizationKey(key: "MUST_CONTAIN_ONE_LOWERCASE_CHAR", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Password must contain one lowercase character"),
                new ("bn", "পাসওয়ার্ডে অবশ্যই একটি ছোট হাতের অক্ষর থাকতে হবে"),
                new ("de", "Das Passwort muss einen Kleinbuchstabencharakter enthalten"),
                new ("fr", "Le mot de passe doit contenir un caractère minuscule"),
            }),

            new LocalizationKey(key: "MUST_NOT_CONTAIN_SPACE", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Password cannot contain spaces"),
                new ("bn", "পাসওয়ার্ডে স্পেস থাকতে পারবে না"),
                new ("de", "Kennwort darf keine Leerzeichen enthalten"),
                new ("fr", "Le mot de passe ne peut pas contenir d'espaces"),
            }),

            new LocalizationKey(key: "MUST_CONTAIN_ONE_SPECIAL_CHAR", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Password must contain one special character"),
                new ("bn", "পাসওয়ার্ডে অবশ্যই একটি বিশেষ অক্ষর থাকতে হবে"),
                new ("de", "Das Passwort muss ein Sonderzeichen enthalten"),
                new ("fr", "Le mot de passe doit contenir un caractère spécial"),
            }),

            new LocalizationKey(key: "PASSWORD_IS_STRONG", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Password is strong"),
                new ("bn", "পাসওয়ার্ড শক্তিশালী"),
                new ("de", "Das Passwort ist sicher"),
                new ("fr", "Le mot de passe est fort"),
            }),

            new LocalizationKey(key: "INVALID_CHARACTERS_IN_FULLNAME", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Invalid characters in full name"),
                new ("bn", "পুরো নামে অবৈধ অক্ষর"),
                new ("de", "Ungültige Zeichen im vollständigen Namen"),
                new ("fr", "Invalid characters in full name"),
            }),

	        #endregion

            #region Game Leaderboard Page

            new LocalizationKey(key: "GameLeaderboardPage_Tagline", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Leaderboard"),
                new ("bn", "লিডারবোর্ড"),
                new ("de", "Bestenliste"),
                new ("fr", "Classement"),
            }),

            new LocalizationKey(key: "GameLeaderboardPage_DailyScoreboardToggle", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "📅 Daily"),
                new ("bn", "📅 দৈনিক"),
                new ("de", "📅 Täglich"),
                new ("fr", "📅 du quotidien"),
            }),

            new LocalizationKey(key: "GameLeaderboardPage_AllTimeScoreboardToggle", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "👑 All time"),
                new ("bn", "👑 সর্বকালে"),
                new ("de", "👑 Alle Zeit"),
                new ("fr", "👑 Tout le temps"),
            }),

            new LocalizationKey(key: "GameLeaderboardPage_PlayNowButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Play now"),
                new ("bn", "এখনই খেলুন"),
                new ("de", "Joue maintenant"),
                new ("fr", "Classement"),
            }),

            new LocalizationKey(key: "LOADING_DATA", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Loading data..."),
                new ("bn", "তথ্য প্রস্তুত হচ্ছে..."),
                new ("de", "Lade Daten..."),
                new ("fr", "Chargement des données..."),
            }),
            new LocalizationKey(key: "NO_DATA_AVAILABLE", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "No data available."),
                new ("bn", "কোন তথ্য নেই।"),
                new ("de", "Keine Daten verfügbar."),
                new ("fr", "Pas de données disponibles."),
            }),

	        #endregion
        };

        #endregion

        #region Methods
        /// <summary>
        /// Gets the localization resource.
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <returns></returns>
        public string GetLocalizedResource(string resourceKey)
        {
            var localizationTemplate = LOCALIZATION_KEYS.FirstOrDefault(x => x.Key == resourceKey);
            return localizationTemplate?.CultureValues.FirstOrDefault(x => x.Culture == App.CurrentCulture).Value;
        }

        /// <summary>
        /// Sets a localized value on the provided ui element.
        /// </summary>
        /// <param name="uIElement"></param>
        public void SetLocalizedResource(UIElement uIElement)
        {
            var localizationTemplate = LOCALIZATION_KEYS.FirstOrDefault(x => x.Key == uIElement.Name);

            if (localizationTemplate is not null)
            {
                var value = localizationTemplate?.CultureValues.FirstOrDefault(x => x.Culture == App.CurrentCulture).Value;

                if (uIElement is TextBlock textBlock)
                    textBlock.Text = value;
                else if (uIElement is TextBox textBox)
                    textBox.Header = value;
                else if (uIElement is PasswordBox passwordBox)
                    passwordBox.Header = value;
                else if (uIElement is Button button)
                    button.Content = value;
                else if (uIElement is ToggleButton toggleButton)
                    toggleButton.Content = value;
                else if (uIElement is HyperlinkButton hyperlinkButton)
                    hyperlinkButton.Content = value;
                else if (uIElement is CheckBox checkBox)
                    checkBox.Content = value;
            }
        }

        #endregion        
    }
}
