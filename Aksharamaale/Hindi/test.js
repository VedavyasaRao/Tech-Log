    let audioCtx = null;

async function playFemaleHindiAudio() {
      const playBtn = document.getElementById('playBtn');
      const textInput = document.getElementById('playbuffertext').value;

      // Initialize AudioContext on user gesture
      if (!audioCtx) {
        audioCtx = new (window.AudioContext || window.webkitAudioContext)();
      } else if (audioCtx.state === 'suspended') {
        await audioCtx.resume();
      }

      playBtn.disabled = true;

      try {
        // Split input text by commas and clean up whitespace/empty entries
        const parts = textInput.split(',').map(p => p.trim()).filter(Boolean);
        if (parts.length === 0) {
          playBtn.disabled = false;
          return;
        }

        const volumeGain = 2.5;
        const targetDurationSec = 3;
        const pauseDurationSec = 1; // 1-second pause per comma

        const decodedBuffers = [];
        let sampleRate = 44100;
        let channels = 1;

        // Fetch and process audio for each part sequentially with a slight delay to prevent proxy rate-limiting
        for (const part of parts) {
          const googleUrl = `https://translate.google.com/translate_tts?ie=UTF-8&q=${encodeURIComponent(part)}&tl=hi&client=tw-ob&ttsspeed=0.9`;
          const proxyUrl = `https://corsproxy.io/?${encodeURIComponent(googleUrl)}`;

          const response = await fetch(proxyUrl);
          if (!response.ok) throw new Error("Network request failed");

          const arrayBuffer = await response.arrayBuffer();
          const originalBuffer = await audioCtx.decodeAudioData(arrayBuffer);

          sampleRate = originalBuffer.sampleRate;
          channels = originalBuffer.numberOfChannels;

          const originalLength = originalBuffer.length;
          const targetLength = Math.max(originalLength, Math.floor(sampleRate * targetDurationSec));
          const leadPadding = Math.floor((targetLength - originalLength) / 2);

          const processedPartBuffer = audioCtx.createBuffer(channels, targetLength, sampleRate);

          for (let channel = 0; channel < channels; channel++) {
            const originalData = originalBuffer.getChannelData(channel);
            const processedData = processedPartBuffer.getChannelData(channel);

            for (let i = 0; i < originalLength; i++) {
              let amplifiedSample = originalData[i] * volumeGain;
              processedData[i + leadPadding] = Math.max(-1.0, Math.min(1.0, amplifiedSample));
            }
          }
          decodedBuffers.push(processedPartBuffer);

          // Small delay between requests to prevent proxy blocks
          await new Promise(resolve => setTimeout(resolve, 150));
        }

        // Calculate total buffer length including 1-second silence gaps
        const silenceSamples = Math.floor(sampleRate * pauseDurationSec);
        let totalLength = 0;
        decodedBuffers.forEach((buf, idx) => {
          totalLength += buf.length;
          if (idx < decodedBuffers.length - 1) {
            totalLength += silenceSamples;
          }
        });

        const finalBuffer = audioCtx.createBuffer(channels, totalLength, sampleRate);
        let offset = 0;

        decodedBuffers.forEach((buf, idx) => {
          for (let channel = 0; channel < channels; channel++) {
            finalBuffer.getChannelData(channel).set(buf.getChannelData(channel), offset);
          }
          offset += buf.length;

          if (idx < decodedBuffers.length - 1) {
            offset += silenceSamples;
          }
        });

        const sourceNode = audioCtx.createBufferSource();
        sourceNode.buffer = finalBuffer;
        sourceNode.connect(audioCtx.destination);

        sourceNode.start(0);

        sourceNode.onended = () => {
          playBtn.disabled = false;
        };

      } catch (err) {
        console.warn("Google TTS fetch failed, switching to queued native speech engine...", err);
        playNativeWebSpeechSequentially(textInput, playBtn);
      }
    }

    // Fallback: Native Browser Speech API queued sequentially with 1-second pauses
    function playNativeWebSpeechSequentially(textInput, playBtn) {
      if (!('speechSynthesis' in window)) {
        playBtn.disabled = false;
        return;
      }

      window.speechSynthesis.cancel();
      const parts = textInput.split(',').map(p => p.trim()).filter(Boolean);
      let currentIndex = 0;

      const voices = window.speechSynthesis.getVoices();
      const femaleVoice = voices.find(v => 
        v.lang.includes('hi') && 
        (v.name.includes('Female') || v.name.includes('Swara') || v.name.includes('Google') || v.name.includes('Kalpana'))
      );

      function speakNext() {
        if (currentIndex >= parts.length) {
          playBtn.disabled = false;
          return;
        }

        const utterance = new SpeechSynthesisUtterance(parts[currentIndex]);
        utterance.lang = 'hi-IN';
        utterance.rate = 0.9;
        utterance.pitch = 1.2;
        if (femaleVoice) utterance.voice = femaleVoice;

        currentIndex++;

        utterance.onend = () => {
          // Wait 1 second before speaking the next item
          setTimeout(speakNext, 1000);
        };

        utterance.onerror = () => {
          playBtn.disabled = false;
        };

        window.speechSynthesis.speak(utterance);
      }

      speakNext();
    }

    if ('speechSynthesis' in window) {
      window.speechSynthesis.onvoiceschanged = () => window.speechSynthesis.getVoices();
    }