# Tests for UI module

## UIUtils.cs

Function | Test Purpose | Test Values | Expected Outcome
--- | --- | --- | ---
enableAndShow | Check if attributes for `visible` and `enable` on UI element are set to `True` | `enableAndShow(UIElement)` | `UIElement` attributes `visible` and `enabled` are `True`
disabledAndHide | check if attributes for `visible` and `enable` on UI element are set to `False` | `disabledAndShow(UIElement)` | `UIElement` attributes `visible` and `enabled` are `False`
showUI | Check if attributes `visible` and `enable` for all elements across a list of UI elements are set to `True` | `showUI(UIList)` | Attributes `visible` and `enabled` for all elements of `UIList` are `True`
hideUI | Check if attributes `visible` and `enable` for all elements across a list of UI elements are set to `False` | `hideUI(UIList)` | Attributes `visible` and `enabled` for all elements of `UIList` are `False`
switchUI | Check if all elements of 'old' UIList are hidden and 'new' UIList are shown | `switchUI(oldUIList, newUIList)` | Attributes `visible` and `enabled` for all elements of `oldUIList` are `False` and `newUIList` are `True`

## SceneManager.cs
Function | Test Purpose | Test Values | Expected Outcome
--- | --- | --- | ---
create | Check if new scene is created correctly | `new Scene(?)` | New scene object is created, and does not return `null`

## UIManager.cs
Function | Test Purpose | Test Values | Expected Outcome
--- | --- | --- | ---
addToUI | Check if UI element has been added to a UIList | `addToUI(UIElement, UIList)` | `UIElement` is a member of `UIList`
updateServerAddress | Check if server address is updated correctly | `updateServerAddress(string)` | Server address now matches `string`
showQRCode | Check if function displays QR code on screen | `showQRCode(image)` | QR code is shown on screen
generateQRCode | Check if correct QR Code is generated | `generateQRCode(string)` | Correct QR Code is generated that represents `string`
