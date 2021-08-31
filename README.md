# TMG1
Тестовое задание 1

Напишите на C# WPF-приложение, подсчитывающее и выводящее количество слов и гласных букв в текстовых строках, полученных со стороннего сервера.
Приложение должно иметь следующий интерфейс: окно, в котором расположены текстовое поле для ввода идентификаторов строк, кнопка для запуска подсчёта и таблица с результатами (см. прилагаемое изображение – interface_mockup.png). Размеры окна могут произвольно изменяться; все прочие элементы при этом должны подстраиваться под новый размер.
В текстовое поле пользователь может ввести произвольное количество идентификаторов строк. Разделителем служит запятая или точка с запятой (пробелы игнорируются – как до, так и после разделителя). Идентификатор строки является целым числом и может принимать значения в диапазоне от 1 до 20 (значения «005», «05» и «5» – эквивалентны). Некорректные идентификаторы должны игнорироваться, при этом должно выводиться предупреждение, а сами такие значения выделяться (подсвечиваться). Дубликаты должны игнорироваться (каждое значение должно учитываться только один раз).
По щелчку по кнопке "Подсчитать", располагающейся рядом с текстовым полем, приложение должно получить искомые строки, произвести подсчёт и вывести результаты.
Результаты должны выводиться в таблице, которая формируется каждый раз при подсчёте. Таблица должна содержать три столбца ("Текст", "Количество слов" и "Количество гласных"), строку заголовка и строки с данными по количеству строк текста.
Адрес сервера, по которому должны отправляться запросы: http://*****.*****.***/api/textstrings/{id}, где {id} – идентификатор строки. По GET-запросу по этому адресу сервер возвращает JSON со следующей структурой {"text":"Some text here."} (поле text содержит искомую строку).
Для аутентификации на сервере необходимо добавить к запросу заголовок «***********» со значением «***************».
Если запрос к серверу не был успешным, необходимо вывести информативное (насколько это возможно) сообщение.
Приложение должно уметь работать с текстом на русском и европейских языках.
Прокомментируйте код на английском (предпочтительно) или русском языке.