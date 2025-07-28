# Field Report Assistant – Demo App

This demo showcases how to build an **on-device AI-powered inspection assistant** using Windows AI Foundry APIs. It is designed for enterprise field workers, such as **construction site inspectors**, who need to capture and process information quickly, securely, and offline.

---

## What It Does

The app helps field inspectors:

- Capture images of site conditions or paper forms
- Extract observations using **on-device OCR** and **Image Description**
- Retrieve related historical issues or standards using **Semantic Search**
- Generate a **inspection summary** using **Phi Silica**
- Customize output to generate a **structured, standardized inspection summary via LoRA-fine-tuning**

All AI features run **locally** — with no dependency on cloud services.

---

## Workflow

1. **Capture Input**
   - Snap a photo of an issue, drawing, or checklist using the device camera
   - Select additional notes or PDFs from the local file system

2. **Extract Observations**
   - Run OCR to extract text from scanned forms or notes
   - Use the Image Description API to quickly add captions for site photos

3. **Search for Related Issues**
   - Use the **Semantic Search API** to query past reports or reference materials
   - Works over a local index of enterprise documents and images

4. **Summarize Findings**
   - Use **Phi Silica** to write a clean, formatted inspection summary
   - Optionally interact via a chat-style Q&A on the device

5. **Format with LoRA**
   - Apply a **LoRA-fine-tuned version of Phi Silica** to convert notes into a structured company-specific report format

---

## Example Output (LoRA-Formatted)

```txt
Field Report - Daily Site Visit
Date: 2025-07-27
Location: South Wing, 3rd Floor

Findings:
- Pipe corrosion detected near HVAC junction
- Fractured concrete slab near elevator shaft
- Safety signage missing in stairwell area

Recommendations:
- Replace affected pipe section
- Reinforce slab and consult structural engineer
- Install OSHA-compliant signage before reopening area