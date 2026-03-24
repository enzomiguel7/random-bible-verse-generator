const API_URL = 'http://localhost:5186'; // Ajuste se sua porta for diferente

// Lista oficial das suas tags
// Lista oficial das suas tags
const ALL_TAGS = [
    'grace', 'weakness', 'power_of_God', 'christ', 'mary', 
    'old_testament', 'new_testament', 'salvation', 'sin', 'prayer', 'wisdom'
];

// Set para armazenar as tags que o usuário selecionou
let selectedTags = new Set();

const textEl = document.getElementById('verse-text');
const refEl = document.getElementById('verse-reference');
const tagsContainer = document.getElementById('available-tags');
const btn = document.getElementById('btn-reveal');

// 1. Gera os botões de tags na tela
function renderTagButtons() {
    tagsContainer.innerHTML = '';
    ALL_TAGS.forEach(tag => {
        const btnTag = document.createElement('button');
        // Estilo Glassmorphism para os botões
        btnTag.className = 'px-4 py-2 rounded-xl bg-white/5 border border-white/10 text-sm transition-all hover:bg-white/10';
        btnTag.innerText = tag.replace(/_/g, ' '); // Troca underline por espaço no visual
        
        // Evento de clique para selecionar/deselecionar
        btnTag.onclick = () => {
            if (selectedTags.has(tag)) {
                selectedTags.delete(tag);
                btnTag.classList.remove('tag-active');
            } else {
                selectedTags.add(tag);
                btnTag.classList.add('tag-active');
            }
        };
        
        tagsContainer.appendChild(btnTag);
    });
}

// 2. Busca o versículo na API usando as tags selecionadas
const verseTagsDisplay = document.getElementById('verse-tags-display'); // Seleciona o novo container

const loadingIndicator = document.getElementById('loading-indicator');

async function loadVerse() {
    try {
        // 1. Inicia o estado de Loading
        btn.disabled = true;
        loadingIndicator.classList.remove('opacity-0', 'pointer-events-none');
        textEl.classList.add('blur-sm'); // Efeito de desfoque no texto antigo

        const startTime = Date.now(); // Marca quando a busca começou

        let url = `${API_URL}/randomverse`;
        if (selectedTags.size > 0) {
            const params = new URLSearchParams();
            selectedTags.forEach(tag => params.append('tag', tag));
            url += `?${params.toString()}`;
        }

        // 2. Faz a chamada para a API
        const response = await fetch(url);
        const data = await response.json();

        // 3. GARANTE UM TEMPO MÍNIMO (ex: 600ms)
        const duration = Date.now() - startTime;
        const minimumWait = 600; 
        
        if (duration < minimumWait) {
            await new Promise(resolve => setTimeout(resolve, minimumWait - duration));
        }

        // 4. Atualiza a Interface após o "delay artificial"
        if (response.status === 200) {
            textEl.innerText = data.text;
            refEl.innerText = data.reference || `${data.book} ${data.chapter}:${data.verse}`;

            verseTagsDisplay.innerHTML = '';
            data.tags.forEach(tag => {
                const span = document.createElement('span');
                span.className = 'px-3 py-1 rounded-lg bg-white/5 border border-white/10 text-[10px] uppercase tracking-widest text-primary/80 font-bold';
                span.innerText = tag.replace(/_/g, ' ');
                verseTagsDisplay.appendChild(span);
            });
        }

    } catch (error) {
        textEl.innerText = "Erro ao carregar versículo.";
    } finally {
        // 5. Finaliza o estado de Loading
        loadingIndicator.classList.add('opacity-0', 'pointer-events-none');
        textEl.classList.remove('blur-sm');
        btn.disabled = false;
    }
}

// Inicialização
renderTagButtons();
btn.addEventListener('click', loadVerse);