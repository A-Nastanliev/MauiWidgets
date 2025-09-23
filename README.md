# MauiWidgets

📱 A .NET MAUI app with Android home screen widgets.  
This project is based on [Lukymistr/MauiWidgets](https://github.com/Lukymistr/MauiWidgets) and extends it with modifications and new widgets.

---

## 📝 Credits

The following widgets were originally created by [Lukymistr](https://github.com/Lukymistr):

- **Light Bulb with Switch**  
- **Light Bulb with Buttons**  
- **Color Button Game**  
- **Date & Time Widget** (modified in this repo)

---

## 🔧 Modifications

- **Date & Time Widget**  
  - The original version used a **timer** and updated every second.
  - This repo replaces the timer with **AlarmManager** (`SetExactAndAllowWhileIdle`), which triggers updates exactly at the start of each minute.  
  - Using the system scheduler ensures the widget always shows the correct time, even if the app is idle or backgrounded.

---

## ➕ Added Widgets

- **Random Color Button**  
  - A simple widget that changes its background color to a new random one when tapped.  

- **Username Label Widget**  
  - Displays the current username set in the app.  
  - Updates automatically whenever the username changes.  

- **Picked Image Widget**  
  - Allows the user to pick an image within the app for the widget to display on the home screen.  
  - Updates automatically whenever the chosen image changes.
---
