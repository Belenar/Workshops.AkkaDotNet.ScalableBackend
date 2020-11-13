/* CONSTANTS */
const ID_SELECT_DEVICE = "select__device";
const ID_SELECT_DATE = "select__date";


/* API */
const fetchDevices = () => {
    axios.get('/api/Devices')
        .then(response => {
            addDevicesToSelect(ID_SELECT_DEVICE, response.data);
        })
        .catch(error => console.error(error));
};
fetchDevices();

const fetchMeasurements = (idDevice, date) => {
    const dateFormatted = date.split('-').reduceRight(
      (accumulator, currentValue) => accumulator.concat('-', currentValue)
    );
    
    axios.get(`/api/Measurements/${idDevice}/${dateFormatted}`)
        .then(response => {
            const data = response.data;

            const {consumption, reading, timestampUtc} = data.reduce((accumulator, item) => {
                Object.keys(item).forEach(key => {
                    accumulator[key] = (accumulator[key] || []).concat(item[key]) 
                })
                return accumulator;
            }, {})

            initChart(consumption, reading, timestampUtc);
        })
        .catch(error => console.error(error));
};

/* FUNCTIONS */
const checkValues = () => {
    const idDevice = $(`#${ID_SELECT_DEVICE}`).val();
    const date = $(`#${ID_SELECT_DATE}`).val();

    if (idDevice && date) {
        fetchMeasurements(idDevice, date);
    }
};

const addDevicesToSelect = (idSelect, devices) => {
    let selectHTML = document.getElementById(idSelect).innerHTML;
    devices.forEach( (device) => {
        selectHTML += "<option value='" + device.id + "'>" + device.name + "</option>";
    });
    document.getElementById(idSelect).innerHTML = selectHTML;
}



/* LISTENERS */
$(`#${ID_SELECT_DEVICE}`).change(() => {
    checkValues();
});

$(`#${ID_SELECT_DATE}`).datepicker({
    autoclose: true,
    endDate: '0d',
    format: 'dd-mm-yyyy',
    todayHighlight: true,
    weekStart: 1
}).on('changeDate', (e) => {
    const datePicked = e.date;
    checkValues();
});


/* CHART */
let chart = null;
const initChart = (consumption, reading, timestampUtc) => {
  if (chart) {
    chart.destroy();
  }

  document.getElementById("chart").style.display = "block";

  const options = {
    series: [{
      name: 'Consumption',
      type: 'column',
      data: consumption
    }, {
      name: 'Readings',
      type: 'line',
      data: reading
    }],
    chart: {
      type: 'line',
      toolbar:{
        tools: {
          download: false
        }
      }
    },
    colors: ['#e09f39', '#e06f27'],
    stroke: { 
      width: [0, 2]
    },
    title: {
      text: ''
    },
    labels: timestampUtc,
    legend: {
      offsetY: 0,
      labels: {
        useSeriesColors: true
      },
      position: 'top',
      horizontalAlign: 'left'
    },
    tooltip: {
      x: {
        format: 'dd MMMM - HH:mm'
      } 
    },
    xaxis: {
      labels: {
        datetimeUTC: false
      },
      tooltip: {
        enabled: false
      },
      type: 'datetime'
    },
    yaxis: [{
      decimalsInFloat: 4,
      title: {
        text: 'Consumption',
      },
    
    }, {
      decimalsInFloat: 2,
      opposite: true,
      min: Math.floor(Math.min(...reading)/100)*100,
      max: Math.ceil(Math.max(...reading)/100)*100,
      title: {
        text: 'Readings'
      }
    }]
  };
  
  chart = new ApexCharts(document.querySelector("#chart"), options);
  chart.render();  
};