<div dir="rtl">  

ראה מסמך זה ב: [English](README.md) | עברית

# פרויקט לוח תקשורת צוות 26
פרויקט תוכנה עבור קורס יסודות הנדסת תוכנה, סמסטר א' של שנה אקדמית תש"ף

### דרישות קדם

הפרויקט דורש ערכת פיתוח (SDK) של <span dir="ltr">.NET Core 3.1</span> עם תמיכה ב-MVC כדי לבנות ולהריץ את המערכת. 

ניתן לפתוח את הפרויקט באמצעות הסביבות הפיתוח הבאות:

Visual Studio 2019

Visual Studio Code

יכול להיות שסביבות נוספות נתמכות אבל זה לא נבדק על ידי המפתחים

## התחלת עבודה

ההנחיות הבאות יגרמו לעותק שלכם של הפרויקט לרוץ על המחשב שלכם עבור מטרות פיתוח ובדיקות.

### שורת פקודה

<div dir="ltr">

```
dotnet build
dotnet run --project BoardProject\BoardProject.csproj
```

</div>

פקודות אלו יפתחו שרת שיהיה ניתן לגשת אליו באמצעות הקישורים: http://localhost:5000 ו- https://localhost:5001 

### Visual Studio

פתחו את הקובץ BoardProject.sln שבתיקיית הבסיס ואז ניתן להפעיל את המערכת באמצעות לחיצה על הכפתור עם המשולש הירוק.

### Visual Studio Code

פתחו את תיקייה הפרויקט ב Visual Studio Code, לחצו F5, אם לא זוהה התוכנה תשאל לגבי התקנה החבילה [C# for Visual Studio Code Extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp), התקינו אותה, ואח"כ יהיה ניתן לבנות ולהפתוח חלון דפדפן כדי להשתמש במערכת ע"י לחיצה על F5.

#### חיבור למסד נתונים

כפי שהוגדר במחלקת [DataContext](https://github.com/peterooch/team26-project/blob/735ee44909c6b4a2f20f1c42e50b934d65c7b4e6/BoardProject/Data/DataContext.cs#L15) שדרכה ניגשים לטבלאות מסד הנתונים, הפרויקט כרגע מוגדר להשתמש במסד נתונים מסוג SQLite שמאוחסן בקובץ `data.db` איפה שהמערכת מופעלת.  
הקובץ נוצר באופן אוטומטי אם אינו קיים.  
המחלקה עצמה יכולה להיות מוגדרת להשתמש במסד נתונים מסוג אחר אך זה דורש שינוי קוד המחלקה ע"י המשתמש.  
## פריסה

ראו [קובץ](https://github.com/peterooch/team26-project/blob/master/.github/workflows/build_deploy.yml) זה כדי לראות דוגמה של פריסה ל Azure App Service בעזרת GitHub Actions.

## נבנה עם
### צד שרת
* [ASP.NET Core MVC](https://github.com/dotnet/aspnetcore) - ספרית צד השרת המרכזית
* [Entity Framework Core](https://github.com/dotnet/efcore) - ספריה למיפוי עצמים למסד הנתונים וחלילה 
* [Json.NET](https://github.com/JamesNK/Newtonsoft.Json) - עבור סִדרוּת של עצמים מ/ל JSON
* [ASP.NET Scaffolding](https://github.com/aspnet/Scaffolding) - שימש עבור יצירת מערכת הניהול
* [Xunit](https://github.com/xunit/xunit) - בשימוש עבור בדיקות יחידה

### צד לקוח
* [Bootstrap](https://getbootstrap.com/) - בשימוש עבור עיצוב ממשק משתמש
* [jscolor color picker](http://jscolor.com/) - בשימוש עבור ממשק של בחירת צבע
* [jQuery](https://jquery.com/) - בשימוש עבור ממשק משתמש דינאמי 
* [Draggable](https://github.com/Shopify/draggable) - בשימוש כדי לאפשר סידור מחדש של אריחים פשוט

## יוצרי המערכת

* **ברוך רוטמן** - [Peterooch](https://github.com/peterooch)
* **רועי אמזלג** - [Roiamz](https://github.com/roiamz)
* **שאולי קראוס** - [Shaul1Kr](https://github.com/Shaul1Kr)
* **מתן ברזי** - [MatanBarazi](https://github.com/MatanBarazi)

</div>