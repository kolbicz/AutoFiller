# AutoFiller
fill out an edit box automatically

this tool will automatically insert the email configured in the registry to the WebEx authentication form (when using ADFS).

just configure the .reg file accordingly.

"TextToSend"="billg@microsoft.com" - the text to send
"WindowTitle"="Meeting Host Account" - the window to look for
"TimeToWait"=dword:000003e8 - delay until the text is sent

those values can be adjusted to do the same with any window. if the focus is not on the editbox, additional code might be needed.
