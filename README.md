# Windows AI Foundry Workshop

This repository contains materials for a **2-hour enterprise workshop on Windows AI Foundry**. The goal of this session is to provide a **hands-on walkthrough** of building intelligent Windows applications powered by the latest **on-device AI APIs** introduced at Build 2025.

---

## Workshop Objectives

Participants will learn how to leverage Windows AI Foundry to:

- **Build Production-Ready AI Applications** using native Windows AI APIs:
  - [Phi Silica](https://learn.microsoft.com/en-us/windows/ai/apis/phi-silica) - Small language model optimized for Copilot+ PCs
  - [Imaging APIs](https://learn.microsoft.com/en-us/windows/ai/apis/imaging) - AI-powered image processing and analysis
  - [OCR](https://learn.microsoft.com/en-us/windows/ai/apis/text-recognition) - Fast and powerful text extraction from images
  - [LoRA Fine-Tuning](https://learn.microsoft.com/en-us/windows/ai/apis/phi-silica-lora) - Tune Phi Silica for structured output or specialized domains
  - [Application Content Semantic Search (private preview)](https://aka.ms/WindowsAISemanticSearch) - "Assignment" to learn more about App Content Indexer and sign up for private preview access

- **Deploy Enterprise AI Solutions** that operate entirely on-device for:
  - Enhanced data privacy and compliance
  - Reduced latency and improved performance
  - Offline capability and reduced cloud dependencies
  - Cost-effective scaling across enterprise deployments

- **Optimize for Copilot+ PCs** to deliver:
  - Hardware-accelerated AI processing via NPU
  - Energy-efficient inference
  - Seamless integration with Windows ecosystem

---

## Business Value Proposition

### For Enterprise Customers
- **Data Sovereignty**: All AI processing remains on-device, ensuring sensitive data never leaves your infrastructure
- **Cost Optimization**: Eliminate recurring cloud AI service costs through on-device processing
- **Performance**: Sub-second response times with hardware-accelerated inference
- **Scalability**: Deploy across thousands of endpoints without additional cloud capacity planning

### For System Integrators & ISVs
- **Competitive Differentiation**: Integrate cutting-edge AI capabilities into existing solutions
- **Rapid Development**: Pre-built APIs reduce development time from months to weeks
- **Flexible Deployment**: Support both online and offline enterprise environments
- **Custom AI Models**: Fine-tune models for specific industry verticals and use cases

### For PC Manufacturers & Resellers
- **Showcase Innovation**: Demonstrate tangible AI capabilities on modern Windows hardware
- **Enterprise Positioning**: Highlight business value of Copilot+ PC investments
- **Solution Selling**: Move beyond hardware specs to complete AI-enabled business solutions

---

## Workshop Experience

### **Hands-On Implementation** (90 minutes)
- Build a complete **AI-powered inspection report system**
- Implement **5 core AI capabilities** with real business applications
- Experience the full development lifecycle from UI to AI integration

### **Use Case Discussion** (30 minutes)
- Open discussion with participants about their specific business needs and applications
- Explore how Windows AI APIs can address real customer challenges
- Collaborative session to identify implementation opportunities

---

## What's Included

### 📁 **demo-app/**
Complete application implementation with:
- **Starter Framework** (`demo-app-start/`) - UI foundation ready for AI integration
- **Complete Solution** (`demo-app-final/`) - Fully functional reference implementation
- **Step-by-Step Guides** (`demo-guides/`) - Detailed implementation documentation

### 📊 **LoRA Fine-Tuning Training Data/**
Sample datasets and examples for:
- Custom model training workflows
- Domain-specific adapter creation
- Training data preparation patterns

### 🔗 **Additional Resources**
- [Windows AI APIs Documentation](https://learn.microsoft.com/en-us/windows/ai/)
- [Copilot+ PC Developer Guide](https://learn.microsoft.com/en-us/windows/ai/copilot-pc)
- [AI Toolkit for Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=ms-windows-ai-studio.windows-ai-studio)
- [Windows AI Dev Gallery](https://github.com/microsoft/ai-dev-gallery)

---

## Getting Started

### **For Immediate Demo**
Navigate to `demo-app/demo-app-final/` and run the complete AI-powered application to see all capabilities in action.

### **For Hands-On Workshop**
Start with `demo-app/demo-app-start/` and follow the guided implementation in `demo-app/demo-guides/`.

### **For Custom Training**
Explore `LoRA Fine-Tuning Training Data/` to understand model customization patterns for your specific business domain.

---


## Technical Requirements

- **Windows 11** (Build 22000+) with latest updates
- **Visual Studio 2022** Professional/Enterprise
- **Copilot+ PC** recommended for optimal performance
- **Windows AI Platform** runtime components

---

This repository serves as your **launchpad for building production-ready on-device AI applications** using Windows AI Foundry. Whether you're evaluating AI capabilities, building proof-of-concepts, or deploying enterprise solutions, these materials provide the foundation for success.