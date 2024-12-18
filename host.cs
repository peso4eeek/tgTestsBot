using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions;
using Telegram.Bot.Requests;

class Host
{
    private static TelegramBotClient _bot;
    private static Dictionary<string, List<Question>> Tests = new Dictionary<string, List<Question>>
     { 
        {"Столицы",  new List<Question>{
            new Question("Столица Индии?", new List<string>{"Магадан", "Нью-Дели", "Париж", "Рим"}, "Нью-Дели"),
            new Question("Столица Японии?", new List<string>{"Бангкок", "Сеул", "Токио", "Киото"}, "Токио"),
            new Question("Столица Франции?", new List<string>{"Вашингтон", "Верлин", "Лондон", "Париж"}, "Париж"),
            new Question("Столица Канады?", new List<string>{"Оттава", "Монреаль", "Торонто", "Ванкувер"}, "Оттава"),
            new Question("Столица Бразилии?", new List<string>{"Сан-паулу", "Бразилиа", "Рио-де-Жанейро", "Буэнос-Айрес"}, "Бразилиа"),
            new Question("Столица Вьетнама?", new List<string>{"Хошимин", "Ханой", "Дананг", "Хайфон"}, "Хошимин"),
            new Question("Столица Эфиопии?", new List<string>{"Найроби", "Бахр-Дар", "Аддис-Абеба", "Хартум"}, "Аддис-Абеба"),
            new Question("Столица Аргентины?", new List<string>{"Буэнос-Айрес", "Сантьяго", "Лима", "Монтевидео"}, "Буэнос-Айрес"),
            new Question("Столица Ливана?", new List<string>{"Триполи", "Дамаск", "Амман", "Бейрут"}, "Бейрут"),
            new Question("Столица Того?", new List<string>{"Абуджа", "Котону", "Бамако", "Ломе"}, "Ломе")

        } }, 
        {"Английский", new  List<Question>{
            new Question("Как на английском сказать 'Стол'?", new List<string>{"Chair", "Table", "Bed", "Desk"}, "Table"),
            new Question("Как на английском сказать 'Мыло'?", new List<string>{"Soap", "Towel", "Shampoo", "Toothbrush"}, "Soap"),
            new Question("Как на английском сказать 'Небо'?", new List<string>{"Sky", "Earth", "Ocean", "Sun"}, "Sky"),
            new Question("Как на английском сказать 'Удивляться'?", new List<string>{"Amuse", "Admire", "Astonish", "Appreciate"}, "Astonish"),
            new Question("Как на английском сказать 'Благодарить'?", new List<string>{"Request", "Inform", "Appreciate", "Assure"}, "Appreciate"),
            new Question("Как на английском сказать 'Преодолевать'?", new List<string>{"Conquer", "Defeat", "Resist", "Overcome"}, "Overcome"),
            new Question("Как на английском сказать 'Обманывать'?", new List<string>{"Mislead", "Deceive", "Fraud", "Cheat"}, "Deceive"),
            new Question("Как на английском сказать 'Недооценивать'?", new List<string>{"Underestimate", "Undervalue", "Underappreciate", "Unrecognizable"}, "Underestimate"),
            new Question("Как на английском сказать 'Слезливый'?", new List<string>{"Whiner", "Lachrymose", "Brat", "Sentimental"}, "Lachrymose"),
            new Question("Как на английском сказать 'Вездесущий'?", new List<string>{"Ubiquitous", "Succinct", "Eloquent", "Serendipity"}, "Ubiquitous")
        } },
        {"История", new  List<Question>{
            new Question("Кто был первым президентом Соединенных Штатов?", new List<string>{"Томас Джефферсон", "Джордж Вашингтон", "Авраам Линкольн", "Джон Адамс"}, "Джордж Вашингтон"),
            new Question("В каком году началась Вторая мировая война?", new List<string>{"1918 год", "1945 год", "1939 год", "1914 год"}, "1939 год"),
            new Question("Какое событие считается началом Русской революции?", new List<string>{"Восстание декабристов", "Февральская революция", "Октябрьская революция", "Расстрел царской семьи"}, "Февральская революция"),
            new Question("Какой римский император легализовал христианство?", new List<string>{"Неро", "Август", "Константин", "Юлий Цезарь"}, "Константин"),
            new Question("Великая хартия вольностей, подписанная в 1215 году, ограничила власть какого монарха?", new List<string>{"Короля Франции", "Императора Священной Римской Империи", "Короля Испании", "Короля Англии"}, "Короля Англии"),
            new Question("Какой орден был учрежден Петром I?", new List<string>{"Орден святой Анны", "Орден святого Георгия", "Орден Андрея Первозванного", "Орден Иоанна Иерусалимкого"}, "Орден Андрея Первозванного"),
            new Question("Кто венчал на царство Ивана Грозного?", new List<string>{"Патриарх Гермоген", "Митриполит Иосаф", "Митрополит Филипп", "Митрополит Макарий"}, "Митрополит Макарий"),
            new Question("Какая династия правила Китаем во времена династии Мин?", new List<string>{"Династия Юань", "Династия Цин", "Династия Хань", "Династия Чжу"}, "Династия Юань"),
            new Question("Чем знаменита Ефросинья, дочь галицкого князя Ярослава Осмомысла?", new List<string>{"Выдала отца половцам", "Не было у него такой дочери", "Отреклась от православной веры", "Упоминалась в 'Слове о полку Игореве'"}, "Упоминалась в 'Слове о полку Игореве'"),
            new Question("Какое сражение, произошедшее в 1066 году, сыграло решающую роль в завоевании Англии норманнами?", new List<string>{"Битва при Гастингсе", "Битва при Азенкуре", "Битва на Босвортском поле", "Битва при Туре"}, "Битва при Гастингсе")
        } }, 
        {"Математика", new List<Question>{
            new Question("Какова формула для определения площади прямоугольника?", new List<string>{"Длина + Ширина", "Длина × Ширина", "(Длина + Ширина) / 2", "Длина × 2"}, "Длина × Ширина"),
            new Question("Чему приблизительно равно значение Pi (π)?", new List<string>{"2.71", "3.14", "1.618", "3"}, "3.14"),
            new Question("Чему равна сумма углов в треугольнике?", new List<string>{"90 градусов", "180 градусов", "270 градусов", "360 градусов"}, "180 градусов"),
            new Question("Какова теорема Пифагора для прямоугольного треугольника со сторонами a, b и гипотенузой c?", new List<string>{"a + b = c", "a² - b² = c²", "a² + b = c", " a² + b² = c²"}, " a² + b² = c²"),
            new Question("Какова формула для вычисления длины окружности круга с радиусом r?", new List<string>{"πr", "πr²", "2πr", "4πr"}, "2πr"),
            new Question("Что такое простое число?", new List<string>{"Число, кратное 2", "Число, кратное 3", "Число, делящееся только на 1 и само на себя", "Четное число"}, "Число, делящееся только на 1 и само на себя"),
            new Question("Какова формула для вычисления объёма сферы с радиусом r?", new List<string>{"4πr²", "(4/3)πr³", "2πr³", "(2/3)πr³"}, "(4/3)πr³"),
            new Question("Как закон синусов связан со сторонами (a, b, c) и противоположными углами (A, B, C) треугольника?", new List<string>{"a/sin(A) = b/sin(B) = c/sin(C)", "a/cos(A) = b/cos(B) = c/cos(C)", "a/tan(A) = b/tan(B) = c/tan(C)", "a² + b² = c²"}, "a/sin(A) = b/sin(B) = c/sin(C)"),
            new Question("Какова формула для вычисления площади поверхности цилиндра с радиусом r и высотой h?", new List<string>{"πr²h", "2πrh", "2πrh + 2πr²", "2πr²h"}, "2πrh + 2πr²"),
            new Question("Согласно формуле Эйлера, какова взаимосвязь между количеством вершин (V), рёбер (E) и граней (F) выпуклого многогранника?", new List<string>{"V + E + F = 2", "V - E + F = 2", "V + E - F = 2", "V - E - F = 2"}, "V - E + F = 2")
        }}
    };

    
    private static Dictionary<long, QuizStates> userState = new Dictionary<long, QuizStates>();

    public Host(string token){
        _bot = new TelegramBotClient(token);

    }


    public void Start(){
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = new [] {UpdateType.Message, UpdateType.CallbackQuery },
        };

        using var cts = new CancellationTokenSource();

        _bot.StartReceiving(
            updateHandler, errorHandler, receiverOptions, cts.Token
        );
    }

    private static async Task updateHandler(ITelegramBotClient client, Update update, CancellationToken token)
    {
        try{
            if (update.Type == UpdateType.Message && update.Message.Text == "/start"){
                await SendQuizSelection(update.Message.Chat.Id, token);
            }
            else if (update.Type == UpdateType.CallbackQuery)
            { 
                await HandleCallbackQuery(update.CallbackQuery, token);
            }
            else {
                await _bot.SendMessage(update.Message.Chat.Id, "Извини, я не могу разобрать что ты мне прислал(");
                await _bot.SendMessage(update.Message.Chat.Id, "Попробуйте написать команду /start чтобы пройти тест");
            }
        }
        catch(Exception e)
        {
            Console.WriteLine($"Ошибка в updateHandler: {e.Message}");
        }
    }

    private static async Task SendQuizSelection(long id, CancellationToken token)
    {

        List<string> KeyList = new List<string>(Tests.Keys);
        var inlineKeyboard = CreateInlineKeyboard(KeyList);
        await  _bot.SendMessage(id, "Выберите тему теста", replyMarkup: inlineKeyboard, cancellationToken: token);

    }

    private static async Task HandleCallbackQuery(CallbackQuery? callbackQuery, CancellationToken token)
    {
        long chatId = callbackQuery.Message.Chat.Id;
        if (!userState.ContainsKey(chatId) )
        {
            await StartQuiz(chatId, callbackQuery.Data);
            await _bot.SendMessage(chatId, "Отлично, давайте начнем!");
            await SendQuestion(chatId, token);
            await _bot.AnswerCallbackQuery(callbackQuery.Id, cancellationToken: token);
            return;
        }

        var quizState = userState[chatId];
        if (quizState.currentQuestionsIndex >= quizState.currentTest.Count)
        {
            await finishQuest(chatId, token);
            return;
        }

        var question = quizState.currentTest[quizState.currentQuestionsIndex];
        quizState.userAnswer.Add(callbackQuery.Data);

         quizState.currentQuestionsIndex++;

        await _bot.AnswerCallbackQuery(callbackQuery.Id, cancellationToken: token);
        await SendQuestion(chatId, token);



    }

    private static InlineKeyboardMarkup CreateInlineKeyboard(List<string> buttons)
    {
        var buttonRows = new List<InlineKeyboardButton[]>();
        int maxRowButtons = 2;
        for (int i = 0; i < Tests.Count; i+= maxRowButtons)
        {
            var rowButtons = new List<InlineKeyboardButton>();
            for (int j = 0; j < maxRowButtons && i + j < Tests.Count; j++)
            {
                var quizName = buttons[i+j];
                rowButtons.Add(InlineKeyboardButton.WithCallbackData(quizName, quizName));
            }
            buttonRows.Add(rowButtons.ToArray());
        }

        return new InlineKeyboardMarkup(buttonRows.ToArray());
    }

    private static async Task SendQuestion(long id, CancellationToken token)
    {
        if (!userState.ContainsKey(id))
        {
            await _bot.SendMessage(id, "Чтобы пройти тест введите команду /start");
            return;
        }
        
        var userQuizState = userState[id];
        if (userQuizState.currentQuestionsIndex >= userQuizState.currentTest.Count)
        {
            await finishQuest(id, token);
        }
        var question = userQuizState.currentTest[userQuizState.currentQuestionsIndex];
        if (userQuizState.currentQuestionsIndex % 3 == 0 &&  userQuizState.currentQuestionsIndex != 0) {await _bot.SendMessage(id, "Неплохо, теперь вопросы посложнее");};
        var inlineKeyboard = CreateInlineKeyboard(question.answerOptions);
        await _bot.SendMessage(id, question.question, replyMarkup: inlineKeyboard, cancellationToken: token);

    }

    private static async Task StartQuiz(long id, string selectedQuizType)
    {
        List<Question> selected =  Tests[selectedQuizType];

        userState[id] = new QuizStates
        {
            currentQuestionsIndex = 0, 
            SelectedQuizType = selectedQuizType,
            currentTest = selected,
        };

    }

    private static Task errorHandler(ITelegramBotClient client, Exception exception, HandleErrorSource source, CancellationToken token)
    {
        var errorMessage = exception switch
        {
             ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
           _ => exception.ToString()
         };

        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }

    private static async Task finishQuest(long id, CancellationToken token)
    {
        var quizState = userState[id];
        int correctAnswer = 0;

        for (int i = 0; i < quizState.currentTest.Count; i++){
            if (quizState.userAnswer[i] == quizState.currentTest[i].correctAnswer)
            {
                correctAnswer++;
            }
        }

        double result = (double)correctAnswer / quizState.currentTest.Count * 100;

        int marks;
        if (result < 20.0) marks = 2;
        else if (result < 60.0) marks = 3;
        else if (result < 85.0) marks = 4;
        else marks = 5;

        await _bot.SendMessage(id, $" ВЫ ответил правильно на {result}% вопросов, ваша оценка {marks}");
        await _bot.SendMessage(id, "Чтобы вернуться в главное меню введите команду /start");
        userState.Remove(id);

    }

    private class Question{
        public string question{ get; }
        public List<string> answerOptions{ get; } = new List<string>();
        public string correctAnswer { get; }

        public Question(string question, List<string> answerOptions, string correctAnswer){
            this.question = question;
            this.answerOptions = answerOptions;
            this.correctAnswer = correctAnswer;
        }
    }

    private class QuizStates{
        public  int currentQuestionsIndex{get; set; }
        public List<String> userAnswer = new List<string>();

        public string? SelectedQuizType {get; set; }
        public List<Question> currentTest{get; set; } = new List<Question>();
    }

    
}