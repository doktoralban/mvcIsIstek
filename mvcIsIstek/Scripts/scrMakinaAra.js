

//<script>
	function Fn_Bul(nx) {
		var input, filter, table, tr, td, i;
		input = document.getElementById("txtAra");
		filter = input.value.toUpperCase();
		table = document.getElementById("myTable");
		tr = table.getElementsByTagName("tr");
		//.................................
		for (i = 0; i < tr.length; i++) {
			for (var c = 0; c < tr[i].cells.length; c++) {
				if (tr[i].cells[c].className = "warning") {
						tr[i].cells[c].className = null;
		}
			}
		}
		//.................................
		for (i = 0; i < tr.length; i++) {
		td = tr[i].getElementsByTagName("td")[nx];
	if (td) {
				if (td.innerHTML.toUpperCase().indexOf(filter) > -1) {
		tr[i].style.display = "";

					//...............
					tr[i].cells[nx].className = "warning";
					//..............
				} else {
		tr[i].style.display = "none";
	}
			}
		}
	}
//</script>