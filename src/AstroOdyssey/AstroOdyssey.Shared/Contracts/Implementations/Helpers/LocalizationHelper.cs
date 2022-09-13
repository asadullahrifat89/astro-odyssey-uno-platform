using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System.Linq;

namespace AstroOdyssey
{
    public class LocalizationHelper: ILocalizationHelper
    {
        #region Fields

        private LocalizationKey[] LOCALIZATION_KEYS = new LocalizationKey[]
        {
            new LocalizationKey(key: "ApplicationName_Header", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Astro Odyssey"),
                new ("bn", "অ্যাস্ট্রো ওডিসি"),
                new ("de", "Astro-Odyssee"),
                new ("fr", "Astro Odyssée"),
            }),

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
                new ("bn", "ভাগ্য নেই"),
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
                new ("en", "🎲 Last Game Score"),
                new ("bn", "🎲 শেষ গেম স্কোর"),
                new ("de", "🎲 Letzter Spielpunkt"),
                new ("fr", "🎲 Score du dernier match"),
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
            //new LocalizationTemplate(key: "COLLECTIBLE_COLLECTED", cultureValues: new (string Culture, string Value)[]
            //{
            //    new ("en", "COMIC BOOK COLLECTED"),
            //    new ("bn", "কমিক বই সংগৃহীত"),
            //    new ("de", "COMIC-BUCH GESAMMELT"),
            //    new ("fr", "BD COLLECTIONNÉE"),
            //}),
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

            new LocalizationKey(key: "GameOverPage_PlayAgainButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Play Again"),
                new ("bn", "আবার খেলুন"),
                new ("de", "Nochmal Abspielen"),
                new ("fr", "Rejouer"),
            }),
            new LocalizationKey(key: "GameOverPage_Tagline", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "GAME OVER"),
                new ("bn", "খেলা শেষ"),
                new ("de", "SPIEL IST AUS"),
                new ("fr", "JEU TERMINÉ"),
            }),
            new LocalizationKey(key: "GameStartPage_AssetsCreditButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "😘 Assets by redfoc.com"),
                new ("bn", "😘 Assets by redfoc.com"),
                new ("de", "😘 Assets von redfoc.com"),
                new ("fr", "😘 Actifs par redfoc.com"),
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

            new LocalizationKey(key: "GameStartPage_DeveloperProfileButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Made with ❤️ by Asadullah Rifat"),
                new ("bn", "আসাদুল্লাহ রিফাত দ্বারা ❤️ দিয়ে তৈরি"),
                new ("de", "Hergestellt mit ❤️ von Asadullah Rifat"),
                new ("fr", "Fait avec ❤️ par Asadullah Rifat"),
            }),

            new LocalizationKey(key: "GameStartPage_PlayButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Start Game"),
                new ("bn", "খেলা শুরু করুন"),
                new ("de", "Spiel Beginnen"),
                new ("fr", "Démarrer Jeu"),
            }),
            new LocalizationKey(key: "GameStartPage_Tagline", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "A classic rock metal ⚡ space shooter on WebAssembly."),
                new ("bn", "WebAssembly-এ একটি ক্লাসিক রক মেটাল ⚡ স্পেস শ্যুটার।"),
                new ("de", "Ein klassischer Rock Metal ⚡ Weltraum-Shooter auf WebAssembly."),
                new ("fr", "Un jeu de tir spatial classique rock métal ⚡ sur WebAssembly."),
            }),
            new LocalizationKey(key: "ShipSelectionPage_ChooseButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Select"),
                new ("bn", "নির্বাচন করুন"),
                new ("de", "Auswählen"),
                new ("fr", "Sélectionner"),
            }),
            new LocalizationKey(key: "ShipSelectionPage_ControlInstructions", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Each one offers a different gameplay experience."),
                new ("bn", "প্রত্যেকে একটি আলাদা গেমপ্লে অভিজ্ঞতা দেয়৷"),
                new ("de", "Jeder bietet ein anderes Spielerlebnis."),
                new ("fr", "Chacun offre une expérience de gameplay différente."),
            }),
            new LocalizationKey(key: "ShipSelectionPage_Tagline", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Select a Ship"),
                new ("bn", "একটি জাহাজ নির্বাচন করুন"),
                new ("de", "Wählen Sie ein Schiff aus"),
                new ("fr", "Sélectionnez un navire"),
            }),

            new LocalizationKey(key: "GameLoginPage_LoginButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Login"),
                new ("bn", "প্রবেশ করুন"),
                new ("de", "Anmeldung"),
                new ("fr", "Connexion"),
            }),

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

            new LocalizationKey(key: "GameLoginPage_RegisterButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "No account yet?"),
                new ("bn", "এখনও কোন অ্যাকাউন্ট নেই?"),
                new ("de", "Noch keinen Account?"),
                new ("fr", "Pas encore de compte?"),
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

            new LocalizationKey(key: "GameSignupPage_SignupButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Signup"),
                new ("bn", "নিবন্ধন করুন"),
                new ("de", "Anmelden"),
                new ("fr", "S'inscrire"),
            }),

            new LocalizationKey(key: "GameOverPage_LeaderboardButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Leaderboard"),
                new ("bn", "লিডারবোর্ড"),
                new ("de", "Bestenliste"),
                new ("fr", "Classement"),
            }),

            new LocalizationKey(key: "GameLeaderboardPage_Tagline", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Leaderboard"),
                new ("bn", "লিডারবোর্ড"),
                new ("de", "Bestenliste"),
                new ("fr", "Classement"),
            }),
            new LocalizationKey(key: "GameLeaderboardPage_PlayNowButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Play now"),
                new ("bn", "এখনই খেলুন"),
                new ("de", "Joue maintenant"),
                new ("fr", "Classement"),
            }),
            new LocalizationKey(key: "GameSignupPage_LoginButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Already have an account?"),
                new ("bn", "ইতিমধ্যে একটি অ্যাকাউন্ট আছে?"),
                new ("de", "Sie haben bereits ein Konto?"),
                new ("fr", "Vous avez déjà un compte?"),
            }),
            new LocalizationKey(key: "GameStartPage_LogoutButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Logout"),
                new ("bn", "প্রস্থান"),
                new ("de", "Ausloggen"),
                new ("fr", "Se déconnecter"),
            }),
            new LocalizationKey(key: "GameStartPage_WelcomeBackText", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "👋 Welcome back!"),
                new ("bn", "👋 স্বাগতম!"),
                new ("de", "👋 Willkommen zurück!"),
                new ("fr", "👋 Content de te revoir!"),
            }),
            new LocalizationKey(key: "GameInstructionsPage_Tagline", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "HOW TO PLAY"),
                new ("bn", "কিভাবে খেলতে হবে"),
                new ("de", "SPIELANLEITUNG"),
                new ("fr", "COMMENT JOUER"),
            }),
            new LocalizationKey(key: "GameInstructionsPage_PlayButton", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "👍 Got it"),
                new ("bn", "👍 বুঝেছি"),
                new ("de", "👍 Ich habs"),
                new ("fr", "👍 J'ai compris"),
            }),

            new LocalizationKey(key: "GameInstructionsPage_ControlsText", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Controls"),
                new ("bn", "নিয়ন্ত্রণ"),
                new ("de", "Kontrollen"),
                new ("fr", "Les contrôles"),
            }),
            new LocalizationKey(key: "GameInstructionsPage_ControlsText2", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Move ⬅️ or ➡️ by pressing the arrow keys on your ⌨️ or by touching the edges of your 📱."),
                new ("bn", "আপনার ⌨ এ তীর কীগুলি টিপে বা আপনার 📱 এর প্রান্তগুলি স্পর্শ করে ⬅ বা ➡ সরান।"),
                new ("de", "Bewegen Sie ⬅️ oder ➡️, indem Sie die Pfeiltasten auf Ihrem ⌨️ oder durch Berühren der Kanten Ihres 📱 📱 drücken."),
                new ("fr", "Déplacez ⬅️ ou ➡️ en appuyant sur les touches de flèche sur votre ⌨️ ou en touchant les bords de votre 📱."),
            }),

            new LocalizationKey(key: "GameInstructionsPage_EnemiesText", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Enemies & meteors"),
                new ("bn", "শত্রু ও উল্কা"),
                new ("de", "Feinde & Meteore"),
                new ("fr", "Ennemis et météores"),
            }),
            new LocalizationKey(key: "GameInstructionsPage_EnemiesText2", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Shoot them and avoid collision."),
                new ("bn", "তাদের গুলি করুন এবং সংঘর্ষ এড়ানো।"),
                new ("de", "Schießen Sie sie und vermeiden Sie Kollision."),
                new ("fr", "Tirez-les et évitez la collision."),
            }),

            new LocalizationKey(key: "GameInstructionsPage_BossesText", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Bosses"),
                new ("bn", "বস"),
                new ("de", "Chefs"),
                new ("fr", "Patrons"),
            }),
            new LocalizationKey(key: "GameInstructionsPage_BossesText2", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Must defeat them to advance to the next level."),
                new ("bn", "পরবর্তী স্তরে অগ্রসর হতে তাদের অবশ্যই পরাজিত করতে হবে।"),
                new ("de", "Muss sie besiegen, um zum nächsten Level voranzukommen."),
                new ("fr", "Doit les vaincre pour passer au niveau supérieur."),
            }),

            new LocalizationKey(key: "GameInstructionsPage_HealthText", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Healths"),
                new ("bn", "স্বাস্থ্য"),
                new ("de", "Gesundheit"),
                new ("fr", "Santé"),
            }),
            new LocalizationKey(key: "GameInstructionsPage_HealthText2", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Collect them to repair your ship."),
                new ("bn", "আপনার জাহাজটি মেরামত করতে এগুলি সংগ্রহ করুন।"),
                new ("de", "Sammeln Sie sie, um Ihr Schiff zu reparieren."),
                new ("fr", "Collectez-les pour réparer votre navire."),
            }),

            new LocalizationKey(key: "GameInstructionsPage_PowerupText", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Powerups"),
                new ("bn", "শক্তি বৃদ্ধি"),
                new ("de", "Einschalten"),
                new ("fr", "Mises sous tension"),
            }),
            new LocalizationKey(key: "GameInstructionsPage_PowerupText2", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Collect them to enforce powerful weapons."),
                new ("bn", "শক্তিশালী অস্ত্র প্রয়োগ করতে এগুলি সংগ্রহ করুন।"),
                new ("de", "Sammeln Sie sie, um mächtige Waffen durchzusetzen."),
                new ("fr", "Les récupérer pour appliquer des armes puissantes."),
            }),

            new LocalizationKey(key: "GameInstructionsPage_CollectiblesText", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Collectibles"),
                new ("bn", "সংগ্রহযোগ্য"),
                new ("de", "Sammlerstücke"),
                new ("fr", "Objets de collection"),
            }),
            new LocalizationKey(key: "GameInstructionsPage_CollectiblesText2", cultureValues: new (string Culture, string Value)[]
            {
                new ("en", "Collect them to increase your firepower."),
                new ("bn", "আপনার ফায়ারপাওয়ারটি বাড়ানোর জন্য এগুলি সংগ্রহ করুন।"),
                new ("de", "Sammeln Sie sie, um Ihre Feuerkraft zu erhöhen."),
                new ("fr", "Collectez-les pour augmenter votre puissance de feu."),
            }),
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
                else if (uIElement is HyperlinkButton hyperlinkButton)
                    hyperlinkButton.Content = value; 
            }
        }

        #endregion        
    }
}
