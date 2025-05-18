# 📡 ESP8266 WiFi Captive Portal

A customizable WiFi captive portal implementation for ESP8266 microcontrollers that demonstrates network security concepts and credential management techniques.

---

## 📖 Overview

This project demonstrates how captive portals work by creating a WiFi access point that redirects all requests to a login page. When users connect to the network and attempt to browse the web, they are presented with a login form that collects credentials.

> **For educational purposes only.** ⚠️

---

## ⚠️ Important Notice

This code is provided **STRICTLY FOR EDUCATIONAL PURPOSES** to demonstrate:

- 🔐 How WiFi captive portals function  
- 📝 How credentials can be collected and stored  
- 🌐 The security implications of connecting to unknown networks  

**DO NOT** use this code for any malicious purposes, including but not limited to:

- 🚫 Unauthorized collection of actual credentials  
- 🚷 Attempting to gain unauthorized access to networks  
- 🎣 Phishing attacks against unsuspecting users  

Misuse of this code may violate laws and regulations related to computer fraud, unauthorized access, and data privacy.

---

## ✨ Features

- 📶 Creates a customizable WiFi access point  
- 🔁 Implements DNS spoofing to redirect all web requests  
- 🧾 Collects WiFi passwords and user IDs through a web form  
- 💾 Stores collected information in persistent EEPROM memory  
- 🛠️ Includes admin interface to view and clear stored credentials  
- 🔄 Persists across power cycles and reboots  

---

## 🛠️ Hardware Requirements

- 🧩 ESP8266-based board (NodeMCU, Wemos D1 Mini, etc.)  
- 🔌 Micro USB cable for programming  
- 🔋 Power source (USB power bank, battery, or USB adapter)  

---

## 💻 Software Dependencies

- 🧰 Arduino IDE  
- 📡 ESP8266WiFi library  
- 🌐 DNSServer library  
- 🖥️ ESP8266WebServer library  
- 🧠 EEPROM library  

---

## 🚀 Installation

1. Install Arduino IDE from [arduino.cc](https://www.arduino.cc/)  
2. Add ESP8266 board support to Arduino IDE:  
   - Open Arduino IDE  
   - Go to **File > Preferences**  
   - Add `http://arduino.esp8266.com/stable/package_esp8266com_index.json` to **Additional Boards Manager URLs**  
   - Go to **Tools > Board > Boards Manager**  
   - Search for `"esp8266"` and install  

3. Clone this repository  
4. Open the project in Arduino IDE  
5. Select your ESP8266 board from **Tools > Board** menu  
6. Connect your ESP8266 and select the appropriate port  
7. Click **Upload**  

---

## ⚙️ Configuration

You can modify the following settings in the code:

- `SSID_NAME`: The name of the WiFi network (default: `"GLA_HOSTEL_5G"`)  
- `SUBTITLE`, `TITLE`, `BODY`: Text shown on the captive portal page  
- `APIP`: The IP address for the access point (default: `192.168.1.21`)  

---

## 🧭 Usage

1. Upload the code to your ESP8266 device  
2. Power the ESP8266  
3. Look for the WiFi network with the configured SSID name  
4. Connect to the network  
5. Open a web browser - you will be automatically redirected to the login page  
6. Access the admin interface at `http://192.168.1.21/pass`  

---

## 🗂️ Project Structure

- **Main code**: Contains the ESP8266 setup and loop functions  
- **Web interface**: HTML/CSS for the captive portal  
- **EEPROM management**: Functions for storing and retrieving credentials  

---

## 🧭 Ethical Guidelines

If you're using this code for security research or education:

- ✅ Always test on your own networks or with explicit permission  
- 🚫 Never collect actual credentials from unsuspecting users  
- 🗑️ Delete collected data after testing  
- 📢 Be transparent about the system's purpose in educational settings  

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## ⚠️ Disclaimer

The author is not responsible for any misuse of this software. This code is provided for educational purposes to understand network security concepts and vulnerabilities. Using this code to collect credentials without explicit consent is illegal and unethical.
