//ApplicationBox
class ApplicationBox extends React.Component {
    state = {
        data: this.props.initialData,
    };

    componentDidMount() {
        this.loadCoinsFromServer;
    };

    loadCoinsFromServer = (callback) => {
        var that = this;

        $.ajax({
            type: "GET",
            contentType: "application/json",
            url: this.props.getCoins,
            success: (r) => {
                if (r && !r.error) {
                    createSucessMessage(r.message);

                    that.updateCoins(r.data);

                    if (typeof callback === 'function')
                        callback();
                }
                else {
                    createErrorMessage(r.message);
                }
            }
        });
    };

    updateCoins = (newValues, callback) => {
        this.setState({
            data: newValues,
        }, () => {
            if (callback) {
                callback();
            }
        });
    };

    saveChanges = (callback) => {
        var coins = this.state.data.map(x => ({
            Value: x.value,
            Quantity: x.quantity,
            UpdatedOn: x.updatedOn,
        }));

        var dataJson = JSON.stringify(coins);

        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: this.props.updateBalance,
            dataType: "JSON",
            data: dataJson,
            success: (r) => {
                if (r && !r.error) {
                    createSucessMessage(r.message);

                    if (typeof callback === 'function') {
                        this.loadCoinsFromServer(callback);
                    } else {
                        this.loadCoinsFromServer();
                    }
                }
                else {
                    createErrorMessage(r.message);
                }
            }
        });
    };

    giveChange = (requestedValue) => {
        var alert = confirm('Any changes made will be saved before the change be process. Click on OK to continue.');
        if (alert == true) {
            var that = this;

            this.saveChanges(() => {
                var dataJson = JSON.stringify(requestedValue);

                $.ajax({
                    type: "POST",
                    contentType: "application/json",
                    url: this.props.giveChangeUrl,
                    dataType: "JSON",
                    data: dataJson,
                    success: (r) => {
                        if (r && !r.error) {
                            createSucessMessage(r.message);

                            that.loadCoinsFromServer();
                        }
                        else {
                            createErrorMessage(r.message);
                        }
                    }
                });
            });
        }
    };

    addCoin = (coin, callback) => {
        var that = this;

        var dataJson = JSON.stringify({
            Value: coin.value,
            Quantity: coin.quantity,
        });
        
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: this.props.addCoin,
            dataType: "JSON",
            data: dataJson,
            success: (r) => {
                if (r && !r.error) {
                    createSucessMessage(r.message);

                    that.loadCoinsFromServer(callback);
                    
                }
                else {
                    createErrorMessage(r.message);
                }
            }
        });
    }

    render() {
        var currentBalance = 0;
        var coinListComponent;
        var changeComponent;
        if (this.state.data && this.state.data.length > 0) {
            currentBalance =  this
                .state
                .data
                .map(coin => coin.quantity * coin.value)
                .reduce((a, b) => a + b);

            coinListComponent =
                <CoinList
                    updateCoins={this.updateCoins}
                    data={this.state.data}
                    currentBalance={currentBalance}
                    saveChanges={this.saveChanges} />

            changeComponent =
                <Change
                    giveChange={this.giveChange}
                    currentBalance={currentBalance} />

        }

        return (
            <div className="applicationBox applicationContainer col-11 container">
                <div className="row bg-primary text-white title">
                    <span className="title">Cash Machine</span>
                </div>
                <AddNewCoinForm
                    data={this.state.data}
                    addCoin={this.addCoin} />
                {coinListComponent}
                {changeComponent}
            </div>
        );
    };
}

//ApplicationBox -> AddNewCoinForm
class AddNewCoinForm extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            value: '',
            quantity: '',
        };
    };

    componentDidMount() {
        $('input[name=addCoinValue]').numeric({ decimal: ".", negative: false, scale: 2 });
        $('input[name=addCoinQuantity]').numeric({ decimal: ".", negative: false, scale: 2 });

    }
    handleValueChange = e => {
        e.preventDefault();

        if (e.target.value && isNaN(e.target.value)) {
            e.target.value = '';
        }

        this.setState({ value: e.target.value });
    };

    handleQuantityChange = e => {
        e.preventDefault();

        if (e.target.value && !Number(e.target.value)) {
            e.target.value = '';
        }

        this.setState({ quantity: e.target.value });
    };

    handleClickAddCoin = e => {
        e.preventDefault();

        var value = Number(this.state.value);
        var quantity = Number(this.state.quantity);

        if (value == 0 || value == '' || quantity == '') {
            createErrorMessage('Fill Informations');
            return;
        }

        if (!isNaN(quantity) && !isNaN(value)) {
            var maxValue = 999999;
            e.preventDefault();

            if (quantity > maxValue) {
                createErrorMessage('Max quantity is: ' + maxValue);
                return;
            }

            if (value > maxValue) {
                createErrorMessage('Max value is: ' + maxValue);
                return;
            }

            if (this.props.data && this.props.data.length > 0) {
                //Verifies if coin with the value inputed is already added
                let duplicatedCoin = this.props.data.filter((c) => {
                    return c.value === value;
                });

                if (duplicatedCoin && duplicatedCoin.length > 0) {
                    createErrorMessage('Coin with value: ' + value + ' already exists');
                    return;
                }
            }

            this.setState({ value: value, quantity: quantity }, () => {
                this.props.addCoin(this.state,
                    () => {
                        this.setState({ value: '', quantity: '' });
                    });
                });
        } else {
            this.setState({ quantity: '', value: '' });
        }
    };  

    render() {
        return (
            <div className="row addCoinForm mB-20px">
                <div className="col-md-4 mL-30px input-group mT-20px">
                    <div className="input-group">
                        <input
                            type="text"
                            maxLength="6"
                            name="addCoinValue"
                            className="form-control"
                            value={this.state.value}
                            onChange={this.handleValueChange}
                            placeholder="Value" />
                        <input
                            type="text"
                            name="addCoinQuantity"
                            className="form-control"
                            maxLength="6"
                            value={this.state.quantity}
                            onChange={this.handleQuantityChange}
                            placeholder="Quantity" />
                        <button
                            className="btn btn-primary btn-md font-weight-bold"
                            onClick={this.handleClickAddCoin}>Add Coin</button>
                    </div>                     
                </div>
            </div>
        );
    };
}

//ApplicationBox -> CoinList
class CoinList extends React.Component {
    constructor(props) {
        super(props);
    };

    componentDidMount() {
        $('.totalValue').numeric({ decimal: ".", negative: false, scale: 2 });
    }

    handleCoinUpdate = (coinUpdated) => {
        //Update coin quantity
        this
            .props
            .data
            .map((coin) => {
                if (coin.value == coinUpdated.value) {
                    coin.quantity = coinUpdated.quantity
                }
            });

        this
            .props
            .updateCoins(this.props.data);
    };

    render() {
        var that = this;
        var coinNodes = this
            .props
            .data
            .map((coin) => {
                return (
                    <Coin
                        handleCoinUpdate={that.handleCoinUpdate}
                        value={coin.value}
                        quantity={coin.quantity}
                        updateOn={coin.updatedOn}
                        key={coin.value}>
                        &nbsp;
                    </Coin>
                );
            });

        return (
            <div>
                <div className="row coinList">
                    <div className="row mT-20px mB-20px">
                        {coinNodes}
                    </div>
                </div>
                <div className="row divTotal offset-5">
                    <div className="col-11">
                        <span className="spanTotal font-weight-bold">Balance:</span>
                        <span className="mL-20px totalValue">{this.props.currentBalance.toFixed(2)}</span>
                    </div>
                </div>
                <div className="row divTotal offset-5">
                    <div className="col-11">
                        <button
                            className="btn btn-primary btn-md font-weight-bold mT-20px"
                            onClick={this.props.saveChanges}>Save changes</button>
                    </div>
                </div>
            </div>
        );
    };
}

//ApplicationBox -> CoinList -> Coin
class Coin extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            quantity: this.props.quantity,
            value: this.props.value,
            updatedOn: this.props.updatedOn,
        };
    };

    rawMarkup = () => {
        var md = createRemarkable();
        var rawMarkup = md.render(this.props.children.toString());
        return { __html: rawMarkup };
    };

    updateQuantity = (quantity) => {
        this.setState({
            quantity
        }, () => {
            this
                .props
                .handleCoinUpdate(this.state);
        });
    };

    render() {
        return (
            <div className="row coinBox">
                <div className="row">
                    <div className="coinInfo">
                        <span className="text-right">Quantity: {this.props.quantity}</span>
                    </div>
                </div>
                 <div className="row mB-20px">
                    <div className="coinInfo">
                        <span className="text-right">Value: {this.state.value.toFixed(2)}</span>
                    </div>
                </div>
                <div className="CoinContainer">
                    <div className="coin">
                        <div className="face heads">
                            <span>{this.state.value.toFixed(2)}</span>
                        </div>
                        <div className="face tails">
                            <span>{this.state.value.toFixed(2)}</span>
                        </div>
                    </div>
                </div>

                <CoinForm
                    updateQuantity={this.updateQuantity}
                    currentQuantity={this.props.quantity}
                    key={this.state.value} />
            </div>
        );
    }
}

//ApplicationBox -> CoinList -> Coin -> CoinForm
class CoinForm extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            inputQuantityValue: '',
            currentQuantity: this.props.currentQuantity
        };
    };

    componentDidMount() {
        $('input[name=inputQuantityValue]').numeric({ negative: false, scale: 0 });
    }

    handleQuantityChange = e => {
        e.preventDefault();
        this.setState({ inputQuantityValue: e.target.value });
    };

    handleClickPlus = e => {
        e.preventDefault();

        var quantityToAdd = Number(this.state.inputQuantityValue);
        if (!isNaN(quantityToAdd)) {
            var maxValue = 999999;
            var currentQuantity = Number(this.props.currentQuantity);
            e.preventDefault();

            var newQuantity = currentQuantity + quantityToAdd;

            if (newQuantity > maxValue) {
                createErrorMessage('Max value is: ' + maxValue);
                return;
            }

            this.setState({
                currentQuantity: newQuantity,
                inputQuantityValue: ''
            }, () => {
                this
                    .props
                    .updateQuantity(this.state.currentQuantity)
            });
        } else {
            this.setState({ inputQuantityValue: '' });
        }
    };

    handleClickRemove = e => {
        e.preventDefault();

        var quantityToRemove = Number(this.state.inputQuantityValue);
        if (!isNaN(quantityToRemove)) {
            var currentQuantity = Number(this.props.currentQuantity);
            var newQuantity = currentQuantity - quantityToRemove;

            if (Number(newQuantity) < 0) {
                createErrorMessage('Minimun value is: 0');
                return;
            }

            this.setState({
                currentQuantity: newQuantity,
                inputQuantityValue: '',
            }, () => {
                this
                    .props
                    .updateQuantity(this.state.currentQuantity);
            });
        } else {
            this.setState({ inputQuantityValue: '' });
        }
    };

    render() {
        return (
            <div className="row coinButtons">
                <div className="">
                    <div className="col-md-12 input-group mT-20px">
                        <button
                            className="btn btn-danger btn-md font-weight-bold w40px"
                            onClick={this.handleClickRemove}>-</button>
                        <input
                            name="inputQuantityValue"
                            type="text"
                            maxLength="6"
                            value={this.state.inputQuantityValue}
                            onChange={this.handleQuantityChange}
                            className="w100px form-control" />
                        <button
                            className="btn btn-primary btn-md font-weight-bold w40px"
                            onClick={this.handleClickPlus}>+</button>
                    </div>
                </div>
            </div>
        );
    };
}

//ApplicationBox -> Change
class Change extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            inputChangeValue: '',
        };
    };

    componentDidMount() {
        $('input[name=giveChangeInput]').numeric({ decimal: ".", negative: false, scale: 2 });;
    }

    handleChangeValue = e => {
        e.preventDefault();

        this.setState({
            inputChangeValue: e.target.value,
        });
    };

    handleClickGiveChange = e => {
        e.preventDefault();

        var quantityToGiveAsChange = Number(this.state.inputChangeValue);
        if (!isNaN(quantityToGiveAsChange)) {
            if (Number(quantityToGiveAsChange) == 0) {
                createErrorMessage("Why should I give you change for 0?");
                return;
            }

            if (Number(quantityToGiveAsChange) > this.props.currentBalance) {
                createErrorMessage('Not enought balance.');
                return;
            }

            this.setState({
                inputChangeValue: '',
            }, () => {
                this
                    .props
                    .giveChange(quantityToGiveAsChange);
            });
        } else {
            this.setState({ inputChangeValue: '' });
        }
    };

    render() {
        return (
            <div className="row">
                <div className="col-md-3 offset-4 input-group mT-20px mB-20px">
                    <input
                        type="text"
                        min="0"
                        maxLength="9"
                        name="giveChangeInput"
                        value={this.state.inputChangeValue}
                        onChange={this.handleChangeValue}
                        className="w100px form-control" />
                    <button
                        className="btn btn-primary btn-md font-weight-bold"
                        disabled={!this.state.inputChangeValue}
                        onClick={this.handleClickGiveChange}>Give Change</button>
                </div>
            </div>
        );
    };
}