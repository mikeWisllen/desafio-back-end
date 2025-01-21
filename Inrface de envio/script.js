document.addEventListener('DOMContentLoaded', () => {
  const form = document.getElementById('messageForm');
  
  form.addEventListener('submit', async (e) => {
      e.preventDefault();
      
      const formData = new FormData(form);
      const data = Object.fromEntries(formData.entries());
      
      try {
          const response = await fetch("http://localhost:5161/api/sms", {
              method: 'POST',
              headers: {
                  'Content-Type': 'application/json',
                  'Accept': 'application/json'
              },
              body: JSON.stringify(data),
              credentials: 'same-origin',
              mode: 'cors'
          });

          const responseData = await response.json();
          console.log('Resposta:', responseData);

          if (response.ok) {
              alert('Mensagem enviada com sucesso!');
              form.reset(); // Limpa o formulário após sucesso
          } else {
              alert('Erro ao enviar a mensagem: ' + responseData.message);
          }
      } catch (error) {
          console.error('Erro completo:', error);
          alert('Não foi possível conectar a API. Verifique se ela está rodando.');
      }
  });
});